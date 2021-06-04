using Business.Helpers;
using Business.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospedagemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody]AccessCredentials credenciais, [FromServices]AccessManager accessManager)
        {
            if (accessManager.ValidateCredentials(credenciais)) return accessManager.GenerateToken(credenciais);
            else return new { Authenticated = false, Message = "Falha ao autenticar" };
        }
    }
}
