using Microsoft.AspNetCore.Mvc;

namespace HighgeekNet.Blazor.Services.SignalR.Snack
{
    [ApiController]
    [Route("api/get")]
    public class SnackController : ControllerBase
    {
        private readonly ISnackService _snackService;

        public SnackController(ISnackService snackService)
        {
            _snackService = snackService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _snackService.CallFromRedis(this, "GET");
            return Ok();
        }
    }
}
