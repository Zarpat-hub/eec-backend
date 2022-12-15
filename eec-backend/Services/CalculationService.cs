using eec_backend.Contracts;
using eec_backend.Contracts.Responses;
using eec_backend.Models;
using System.Linq;

namespace eec_backend.Services
{
    public interface ICalculationService
    {
        Task<BaseResponse> GetCalculation(BaseRequest baseRequest);
    }

    public class CalculationService : ICalculationService
    {
        private readonly IProductService _productService;

        public CalculationService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<BaseResponse> GetCalculation(BaseRequest baseRequest)
        {
            
            Product product = await _productService.GetProductById(baseRequest.ModelIdentifier);
            BaseResponse response = new BaseResponse();
            response.AnnualCost = GetAnnualCost(product, baseRequest);
            response.EcoScore = GetEcoScore(product);
            response.EnergyEfficiencyClass = product.EnergyEfficiencyClass;
            response.Category = product.Category;
            response.Manufacturer = product.SupplierOrTrademark;
            response.PowerConsumption = product.EnergyConsumption;
            response.ModelIdentifier = product.ModelIdentifier;
            response.DeviceName = baseRequest.DeviceName;
            
            var recc = ReccommendedProducts(product);
            Dictionary<string, List<SingleCalculationModel>> upgrades = new();

            foreach(var item in recc)
            {
                upgrades.Add(item.Key, new List<SingleCalculationModel>
                    (item.Value.Select(x => new SingleCalculationModel()
                    {
                        AnnualCost = GetAnnualCost(x, baseRequest),
                        EcoScore = GetEcoScore(x),
                        EnergyEfficiencyClass = x.EnergyEfficiencyClass,
                        Category = x.Category,
                        Manufacturer = x.SupplierOrTrademark,
                        ModelIdentifier = x.ModelIdentifier,
                        PowerConsumption = x.EnergyConsumption,
                        DeviceName = $"{baseRequest.DeviceName}_Upgraded"
                    })).ToList());
            }

            response.Upgrades = upgrades;

            return response;
        }
        
        private Dictionary<string, List<Product>> ReccommendedProducts(Product product)
        {
            IEnumerable<Product> allProductsInCategory = _productService.GetAllProducts().Result.Where(p => p.Category == product.Category);
            var reccommendations = allProductsInCategory
                .Where(p => GetSimilarParametersProducts(allProductsInCategory, product).Contains(p) && GetProductsWithBetterEnergyClass(allProductsInCategory, product).Contains(p))
                .GroupBy(p => p.EnergyEfficiencyClass[..1])
                .ToDictionary(p => p.Key, p => p.OrderBy(x => x.EnergyEfficiencyIndex).Take(3).ToList());
                
            return reccommendations;
        }

        private double GetAnnualCost(Product product, BaseRequest request)
        {
            Enum category = GetCategoryEnum(product);
            if (category is CategoriesEnum.OVEN || category is CategoriesEnum.WASHING_MACHINE || category is CategoriesEnum.DISHWASHER)
            {
                if (request.WeeklyCycles == null)
                {
                    throw new ArgumentException($"This product category ${product.Category} should have specified weekly cycles");
                }

                double cost = (double)(product.EnergyConsumption * request.WeeklyCycles * 4 * 12 * request.EnergyPrice);
                cost += (double)(product.WaterConsumption != null ? (product.WaterConsumption/1000.0) * request.WeeklyCycles * 4 * 12 * request.WaterPrice : 0);

                return cost;
            }

            return product.EnergyConsumption * request.EnergyPrice;
        }

        private double GetEcoScore(Product product)
        {
            double score = GetCategoryEnum(product) switch
            {
                CategoriesEnum.REFRIGERATOR => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 44, 125) - 44) / (125 - 44)),
                CategoriesEnum.OVEN => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 62, 120) - 62) / (120 - 62)),
                CategoriesEnum.AIR_CONDITIONER => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 2.60, 8.50) - 8.50) / (2.60 - 8.50)),
                CategoriesEnum.WASHING_MACHINE => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 52, 92) - 52) / (92 - 52)),
                CategoriesEnum.DISHWASHER => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 32, 62) - 32) / (62 - 32)),
            };
            return Math.Round(score * 100, 3);
        }

        private static CategoriesEnum GetCategoryEnum(Product product)
        {
            return product.Category switch
            {
                "refrigeratingappliances2019" => CategoriesEnum.REFRIGERATOR,
                "ovens" => CategoriesEnum.OVEN,
                "airconditioners" => CategoriesEnum.AIR_CONDITIONER,
                "washingmachines2019" => CategoriesEnum.WASHING_MACHINE,
                "dishwashers2019" => CategoriesEnum.DISHWASHER,
                _ => throw new NotSupportedException($"Given category ${product.Category} is not currently supported"),
            };
        }

        private enum CategoriesEnum
        {
            REFRIGERATOR,
            OVEN,
            AIR_CONDITIONER,
            WASHING_MACHINE,
            DISHWASHER
        }

        private IEnumerable<Product> GetProductsWithBetterEnergyClass(IEnumerable<Product> products, Product givenProduct)
        {
            string[] classesByAlphabetical = { "A", "B", "C", "D", "E", "F", "G" };
            Index givenProductEnergyClassIndex = Array.IndexOf(classesByAlphabetical, givenProduct.EnergyEfficiencyClass);
            var slice = classesByAlphabetical[..givenProductEnergyClassIndex];
            IEnumerable<Product> betterClassesProducts = products.Where(p => slice.Contains(p.EnergyEfficiencyClass));
            return betterClassesProducts;
        }

        private IEnumerable<Product> GetSimilarParametersProducts(IEnumerable<Product> products, Product givenProduct)
        {
            CategoriesEnum categoriesEnum = GetCategoryEnum(givenProduct);

            return categoriesEnum switch
            {
                CategoriesEnum.REFRIGERATOR => products.Where(p => p.DesignType == givenProduct.DesignType && CheckDimensionsSimilarity(givenProduct, p)),
                CategoriesEnum.OVEN => products.Where(p => p.EnergySource == givenProduct.EnergySource),
                CategoriesEnum.AIR_CONDITIONER => products.Where(p => p.EnergyConsumption <= givenProduct.EnergyConsumption),
                CategoriesEnum.WASHING_MACHINE => products.Where(p => p.DesignType == givenProduct.DesignType && CheckDimensionsSimilarity(givenProduct, p)),
                CategoriesEnum.DISHWASHER => products.Where(p => p.DesignType == givenProduct.DesignType && CheckDimensionsSimilarity(givenProduct, p))
            };

            bool CheckDimensionsSimilarity(Product givenProduct, Product productToCompare)
            {
                if
                (
                    givenProduct.DimensionWidth - 10 > productToCompare.DimensionWidth ||
                    givenProduct.DimensionWidth + 10 < productToCompare.DimensionWidth ||
                    givenProduct.DimensionHeight - 10 > productToCompare.DimensionHeight ||
                    givenProduct.DimensionHeight + 10 < productToCompare.DimensionHeight ||
                    givenProduct.DimensionDepth - 10 > productToCompare.DimensionDepth ||
                    givenProduct.DimensionDepth + 10 > productToCompare.DimensionDepth
                )
                {
                    return false;
                }

                return true;
            }
        }
    }
}
