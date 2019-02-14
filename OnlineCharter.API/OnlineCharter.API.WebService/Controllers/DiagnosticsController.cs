using Microsoft.AspNetCore.Mvc;

namespace OnlineCharter.API.WebService.Controllers
{
    public class DiagnosticsController
    {
        [HttpGet]
        [Route("/ping")]
        public IActionResult Ping() => new OkResult();
    }
}
