using eec_backend.Contracts;
using eec_backend.Contracts.Responses;
using eec_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eec_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService _calculationService;
        private readonly ILogger<CalculationController> _logger;

        public CalculationController(ICalculationService calculationService, ILogger<CalculationController> logger)
        {
            _calculationService = calculationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> GetCalculations([FromBody]BaseRequest request)
        {
            try
            {
                BaseResponse response = await _calculationService.GetCalculation(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "\n", ex.StackTrace);
                return StatusCode(500, "An error occured. Please check application logs for more details");
            }
        }
    }
}
