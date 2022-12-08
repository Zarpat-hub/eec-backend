using eec_backend.Models;

namespace eec_backend.Contracts.Responses
{
    public class BaseResponse
    {
        public SingleCalculationModel Given { get; set; }
        public Dictionary<string, List<SingleCalculationModel>> Upgrades { get; set; }
    }
}
