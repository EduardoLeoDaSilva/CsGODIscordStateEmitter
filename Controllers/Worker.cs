using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsGOStateEmitter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Worker : ControllerBase
    {
        private readonly DiscordEmitter _discordEmitter;
        public Worker(DiscordEmitter discordEmitter)
        {
            _discordEmitter = discordEmitter;
        }

        [HttpGet]
        public IActionResult StartWorker()
        {
            RecurringJob.AddOrUpdate(
                "OperationProcessorWorker",
                () => _discordEmitter.SendMessage(),
                 Cron.Minutely());

            return Ok();
        }
    }
}
