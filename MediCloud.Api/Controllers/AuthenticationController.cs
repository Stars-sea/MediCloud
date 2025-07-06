using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Contracts.Authentication;
using MediCloud.Domain.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController(
    IRequestClient<LoginQuery>      loginRequestClient,
    IRequestClient<RegisterCommand> registerRequestClient
) : ApiController {

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        Response loginResponse = await loginRequestClient.GetResponse<AuthenticationResult, Error>(
            new LoginQuery(request.Email, request.Password)
        );
        return loginResponse switch {
            (_, AuthenticationResult result) => MapResultToResponse(result),
            (_, Error error)                 => Problem(error),
            _                                => throw new InvalidOperationException()
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        Response registerResponse = await registerRequestClient.GetResponse<AuthenticationResult, Error>(
            new RegisterCommand(request.Username, request.Email, request.Password)
        );

        return registerResponse switch {
            (_, AuthenticationResult result) => MapResultToResponse(result),
            (_, Error error)                 => Problem(error),
            _                                => throw new InvalidOperationException()
        };
    }

    [NonAction]
    private static AuthenticationResponse MapResultToResponse(AuthenticationResult result) {
        return new AuthenticationResponse(
            result.User.Id,
            result.User.Email!,
            result.User.UserName!,
            result.Token
        );
    }

}
