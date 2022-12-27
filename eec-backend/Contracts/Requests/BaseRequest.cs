using Newtonsoft.Json;
using System.ComponentModel;

namespace eec_backend.Contracts
{
    public class BaseRequest
    {
        [JsonProperty]
        public string ModelIdentifier { get; set; } = null!;
        [JsonProperty]
        public string DeviceName { get; set; } = "MyDevice";
        [JsonProperty]
        public double EnergyPrice { get; set; } = 0.77f;
        [JsonProperty]
        public double WaterPrice { get; set; } = 3.19f;
        [JsonProperty]
        public int? WeeklyCycles { get; set; }
    }
}
