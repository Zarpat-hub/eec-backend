using eec_backend.Contracts;
using eec_backend.Contracts.Responses;
using eec_backend.Models;

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

            BaseResponse baseResponse = new()
            {
                AnnualCost = GetAnnualCost(product, baseRequest),
                EcoScore = GetEcoScore(product),
            };
            
            return baseResponse;
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
                CategoriesEnum.REFRIGERATOR => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 41, 125) - 41) / (125 - 41)),
                CategoriesEnum.OVEN => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 45, 159) - 45) / (159 - 45)),
                CategoriesEnum.AIR_CONDITIONER => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 2.60, 8.50) - 8.50) / (2.60 - 8.50)),
                CategoriesEnum.WASHING_MACHINE => 1 - ((Math.Clamp(product.EnergyEfficiencyIndex, 52, 102) - 52) / (102 - 52)),
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
    }
}
