using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDB.Architecture.AppException.Model;
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

        /// <summary>
        /// Validates given username and password againts authentication db and creates an authentication token if supplied credentials are valid.
        /// </summary>
        /// <response code="200">User is valid and token is created.</response>
        /// <response code="400">Supplied credentials are wrong.</response>
        /// <param name="request">Request body.</param>
        /// <returns>An object containing token data and role of user.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateTokenResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(CreateTokenRequestModel request)
        {
            return Ok(this.TokenManager.CreateToken(request));
        }
    }
}
