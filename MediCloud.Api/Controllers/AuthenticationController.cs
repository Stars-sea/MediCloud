using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Contracts;
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
        Response<Result<AuthenticationResult>> loginResponse =
            await loginRequestClient.GetResponse<Result<AuthenticationResult>>(
                new LoginQuery(request.Email, request.Password)
            );

        return loginResponse.Message.Match(
            r => Ok(MapResultToResponse(r)), Problem
        );
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        Response<Result<AuthenticationResult>> registerResponse =
            await registerRequestClient.GetResponse<Result<AuthenticationResult>>(
                new RegisterCommand(request.Username, request.Email, request.Password)
            );

        return registerResponse.Message.Match(
            r => Ok(MapResultToResponse(r)), Problem
        );
    }

    [NonAction]
    private static AuthenticationResponse MapResultToResponse(AuthenticationResult result) {
        return new AuthenticationResponse(
            result.User.Id.ToString(),
            result.User.Email,
            result.User.Username,
            result.Token
        );
    }

}
