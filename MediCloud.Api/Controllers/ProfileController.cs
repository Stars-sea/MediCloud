using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MassTransit.Mediator;
using MediCloud.Api.Common.Mappers;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Contracts.Profile;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("profiles")]
public class ProfileController(
    IMediator mediator
) : ApiController {

    [HttpGet("me")]
    public async Task<ActionResult<MyProfileResponse>> GetMe() {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var findResult = await mediator.SendRequest(new FindUserByIdQuery(id));
        if (!findResult.IsSuccess) return Problem(findResult.Errors);

        User user = findResult.Value!;

        long expires = Convert.ToInt64(
            User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value
        );
        DateTimeOffset expiresOffset = DateTimeOffset.FromUnixTimeSeconds(expires);

        return Ok(user.MapDetailedResp(expiresOffset));
    }

    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MyProfileResponse))]
    public async Task<ActionResult> GetProfile(string username) {
        UserId? id = TryGetUserId();
        long expires = Convert.ToInt64(
            User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value
        );
        DateTimeOffset expiresOffset = DateTimeOffset.FromUnixTimeSeconds(expires);

        var findResult = await mediator.SendRequest(new FindUserByNameQuery(username));
        return findResult.Match(
            user => user.Id == id
                ? Ok(user.MapDetailedResp(expiresOffset))
                : Ok(user.MapResp()),
            Problem
        );
    }

    [HttpPost("password")]
    public async Task<ActionResult> SetPassword([FromBody] ChangePasswordRequest request) {
        string email = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;

        var setPasswordResult = await mediator.SendRequest(request.ToCommand(email));
        return setPasswordResult.Match<ActionResult>(Ok, Problem);
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult<DeleteResponse>> Delete(string username, [FromBody] DeleteRequest request) {
        string email = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;

        var deleteResult = await mediator.SendRequest(request.ToCommand(username, email));
        return deleteResult.Match(() => Ok(new DeleteResponse(username, email)), Problem);
    }

}
