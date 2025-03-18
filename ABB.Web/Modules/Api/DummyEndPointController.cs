using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Api
{
    [ApiController]
    [Route("api/Dummy")]
    public class DummyEndPointController : BaseController
    {
        [HttpPost("DataAkseptasi")]
        public IActionResult DataAkseptasi()
        {
            return Ok("done");
        }
    }
}