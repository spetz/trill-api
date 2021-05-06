using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trill.Core;

namespace Trill.Api.Controllers
{
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly IOptions<ApiOptions> _options;

        public HomeController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<string> Get() => Ok(_options.Value.Name);
    }
}