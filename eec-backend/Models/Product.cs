﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace eec_backend.Models
{
    public class Product
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("modelIdentifier")]
        public string ModelIdentifier { get; set; } = null!;

        [Required]
        [JsonProperty("supplier")]
        public string SupplierOrTrademark { get; set; } = null!;

        [Required]
        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("dimensionWidth", NullValueHandling = NullValueHandling.Ignore)]
        public double? DimensionWidth { get; set; }

        [JsonProperty("dimensionHeight", NullValueHandling = NullValueHandling.Ignore)]
        public double? DimensionHeight { get; set; }

        [JsonProperty("dimensionDepth", NullValueHandling = NullValueHandling.Ignore)]
        public double? DimensionDepth { get; set; }

        [Required]
        [JsonProperty("energyEfficiencyClass")]
        public string EnergyEfficiencyClass { get; set; } = null!;

        [Required]
        [JsonProperty("energyEfficiencyIndex")]
        public double EnergyEfficiencyIndex { get; set; }

        [JsonProperty("designType", NullValueHandling = NullValueHandling.Ignore)]
        public string? DesignType { get; set; }

        [Required]
        [JsonProperty("energyConsumption")]
        public double EnergyConsumption { get; set; }

        [JsonProperty("waterConsumption", NullValueHandling = NullValueHandling.Ignore)]
        public double? WaterConsumption { get; set; }

        [JsonProperty("energySource", NullValueHandling = NullValueHandling.Ignore)]
        public string? EnergySource { get; set; }

        [JsonProperty("ratedCapacity", NullValueHandling = NullValueHandling.Ignore)]
        public double? RatedCapacity { get; set; }
    }
}
