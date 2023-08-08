using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.APP.Controllers
{
    [Route("[controller]")] 
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Version()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;

            return Ok(new 
            {
                version = version
            });
        }
    }
}