using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using VDB.Architecture.AppException.Manager;
using VDB.Architecture.AppException.Model;
using VDB.Architecture.Concern.ExtensionMethods;

namespace VDB.MicroServices.CVEData.Web.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ExceptionParser ExceptionParser;
        private readonly ILogger<ErrorController> Logger;

        public ErrorController(ExceptionParser exceptionParser, ILogger<ErrorController> logger)
        {
            this.ExceptionParser = exceptionParser;
            this.Logger = logger;
        }

        [Route(nameof(Error))]
        public IActionResult Error()
        {
            IExceptionHandlerFeature handler = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ParsedException parsedException = this.ExceptionParser.Parse(handler.Error);

            if (!String.IsNullOrWhiteSpace(parsedException.LogID))
            {
                this.Logger.LogError(handler.Error, parsedException.SerializeAsJson());
            }

            return new ObjectResult(new { parsedException.LogID, parsedException.Messages })
            {
                StatusCode = (int)parsedException.StatusCode
            };
        }
    }
}
