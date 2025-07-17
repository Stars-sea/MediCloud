using System.IdentityModel.Tokens.Jwt;
using MediCloud.Api.Common.Http;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[ApiController]
[Authorize]
public abstract class ApiController : ControllerBase {

    [NonAction]
    protected UserId? TryGetUserId() {
        try {
            string userId = User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;
            return UserId.Factory.Create(Guid.Parse(userId));
        }
        catch (Exception e) { return null; }
    }

    [NonAction]
    protected ObjectResult Problem(params IEnumerable<Error> errors) {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        Error? firstError = errors.FirstOrDefault();
        int statusCode = firstError?.Type switch {
            ErrorType.Unexpected   => StatusCodes.Status400BadRequest,
            ErrorType.Validation   => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden    => StatusCodes.Status403Forbidden,
            ErrorType.NotFound     => StatusCodes.Status404NotFound,
            ErrorType.Conflict     => StatusCodes.Status409Conflict,
            _                      => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, title: firstError?.Description);
    }

}
