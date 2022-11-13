using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eec_backend.Models
{
    public class Product
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? ModelIdentifier { get; set; }

        [Required]
        public string? SupplierOrTrademark { get; set; }

        [Required]
        public string? Category { get; set; }

        public double? DimensionWidth { get; set; }

        public double? DimensionHeight { get; set; }

        public double? DimensionDeptht { get; set; }

        public string? EnergyEfficiencyClass { get; set; }

        public double EnergyEfficiencyIndex { get; set; }

        public string? DesignType { get; set; }

        public double EnergyConsumption { get; set; }

        public string? EnergySource { get; set; }

        public double? RatedCapacity { get; set; }
    }
}
