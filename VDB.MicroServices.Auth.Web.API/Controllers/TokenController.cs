using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.MicroServices.Auth.Manager.Business.Interface;
using VDB.MicroServices.Auth.Model.Exchange.Token.Create;

namespace VDB.MicroServices.Auth.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenManager TokenManager;

        public TokenController(ITokenManager tokenManager)
        {
            this.TokenManager = tokenManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(CreateTokenRequestModel request)
        {
            return Ok(this.TokenManager.CreateToken(request));
        }
    }
}
