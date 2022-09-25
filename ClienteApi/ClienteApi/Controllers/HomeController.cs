using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClienteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("teste")]
        [Authorize]
        [HavePermissionAsync("Teste")]
        public string Teste()
        {
            return "OK";
        }

        [HttpGet("teste2")]
        [Authorize]
        [HavePermissionAsync("Teste2")]
        public string Teste2()
        {
            return "OK";
        }

        [HttpGet("teste3")]
        [AllowAnonymous]
        public string Teste3()
        {
            return "OK";
        }
    }
}
