namespace eec_backend.Models
{
    public class SingleCalculationModel
    {
        public string ModelIdentifier { get; set; }
        public string DeviceName { get; set; } = "MyDevice";
        public double AnnualCost { get; set; }
        public string EnergyEfficiencyClass { get; set; }
        public double EcoScore { get; set; }
        public string Category { get; set; }
        public double PowerConsumption { get; set; }
        public string Manufacturer { get; set; }
    }
}
