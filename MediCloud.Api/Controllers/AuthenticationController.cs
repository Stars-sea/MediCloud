using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Contracts.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("auth")]
public class AuthenticationController(
    IMediator mediator
) : ApiController {

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        var loginResult = await mediator.SendRequest(new LoginQuery(request.Email, request.Password));

        return loginResult.Match(
            r => Ok(MapResultToResponse(r)), Problem
        );
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        var registerResult = await mediator.SendRequest(
            new RegisterCommand(request.Username, request.Email, request.Password)
        );

        return registerResult.Match(
            r => Ok(MapResultToResponse(r)), Problem
        );
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<AuthenticationResponse>> Refresh() {
        string email        = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;
        string jti          = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        string expiresStamp = User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value;

        var refreshResult = await mediator.SendRequest(new RefreshCommand(email, jti, expiresStamp));
        return refreshResult.Match(
            r => Ok(MapResultToResponse(r)), Problem
        );
    }

    [NonAction]
    private static AuthenticationResponse MapResultToResponse(AuthenticationResult result) {
        return new AuthenticationResponse(
            result.User.Id.ToString(),
            result.User.Email,
            result.User.Username,
            result.Token,
            result.Expires
        );
    }

}
