using ErrorOr;
using MediCloud.Api.Common.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase {
    
    [NonAction]
    protected ObjectResult Problem(params List<Error> errors) {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        Error firstError = errors.FirstOrDefault();
        int statusCode = firstError.Type switch {
            ErrorType.Validation   => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.NotFound     => StatusCodes.Status404NotFound,
            ErrorType.Forbidden    => StatusCodes.Status403Forbidden,
            ErrorType.Conflict     => StatusCodes.Status409Conflict,
            _                      => StatusCodes.Status500InternalServerError,
        };
        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}
