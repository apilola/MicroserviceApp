using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        AutoMapper.IMapper _autoMapper;

        public PlatformsController()
        {

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine($"--> {nameof(TestInboundConnection)}");

            return Ok($"Inbound test from {nameof(PlatformsController)}");
        }

    }
}