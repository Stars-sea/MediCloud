using ErrorOr;
using MediCloud.Application.Services.Authentication;
using MediCloud.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("auth")]
public class AuthenticationController(
    IAuthenticationService authenticationService
) : ApiController {
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        ErrorOr<AuthenticationResult> authResult =
            await authenticationService.LoginAsync(request.Email, request.Password);

        return authResult.Match(
            result => Ok(MapResultToResponse(result)),
            Problem
        );
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        ErrorOr<AuthenticationResult> authResult =
            await authenticationService.RegisterAsync(request.Username, request.Email, request.Password);

        return authResult.Match(
            result => Ok(MapResultToResponse(result)),
            Problem
        );
    }

    [NonAction]
    private static AuthenticationResponse MapResultToResponse(AuthenticationResult result)
        => new(
            result.User.Id,
            result.User.Email!,
            result.User.UserName!,
            result.Token
        );
}
