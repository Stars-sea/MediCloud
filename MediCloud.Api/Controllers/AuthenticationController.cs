using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MassTransit.Mediator;
using MediCloud.Api.Common.Mappers;
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

    [NonAction]
    private CreatedResult TokenCreated(AuthenticationResult result) {
        return Created("/auth/token", result.MapResp());
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        var loginResult = await mediator.SendRequest(request.MapQuery());
        return loginResult.Match(TokenCreated, Problem);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        var registerResult = await mediator.SendRequest(request.MapCommand());
        return registerResult.Match(TokenCreated, Problem);
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<AuthenticationResponse>> Refresh() {
        string email        = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;
        string jti          = User.FindFirst(JwtRegisteredClaimNames.Jti)!.Value;
        string expiresStamp = User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value;

        var refreshResult = await mediator.SendRequest(new RefreshTokenCommand(email, jti, expiresStamp));
        return refreshResult.Match(TokenCreated, Problem);
    }

}
