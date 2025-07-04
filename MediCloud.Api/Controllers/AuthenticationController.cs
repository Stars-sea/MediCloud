using ErrorOr;
using MediatR;
using MediCloud.Application.Authentication.Commands.Register;
using MediCloud.Application.Authentication.Common;
using MediCloud.Application.Authentication.Queries.Login;
using MediCloud.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("auth")]
public class AuthenticationController(
    ISender sender
) : ApiController {
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        ErrorOr<AuthenticationResult> authResult =
            await sender.Send(new LoginQuery(request.Email, request.Password));

        return authResult.Match(
            result => Ok(MapResultToResponse(result)),
            Problem
        );
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        ErrorOr<AuthenticationResult> authResult =
            await sender.Send(new RegisterCommand(request.Username, request.Email, request.Password));

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
