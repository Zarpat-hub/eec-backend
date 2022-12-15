using eec_backend.Models;

namespace eec_backend.Contracts.Responses
{
    public class BaseResponse
    {
        public string ModelIdentifier { get; set; } //Thanks for this code to frontend mocks :)
        public string DeviceName { get; set; }
        public double AnnualCost { get; set; }
        public string EnergyEfficiencyClass { get; set; }
        public double EcoScore { get; set; }
        public string Category { get; set; }
        public double PowerConsumption { get; set; }
        public string Manufacturer { get; set; }
        public Dictionary<string, List<SingleCalculationModel>> Upgrades { get; set; }
    }
}
