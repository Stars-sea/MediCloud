using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Contracts.Profile;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("profile")]
public class ProfileController(
    IMediator mediator
) : ApiController {

    [HttpGet("me")]
    public async Task<ActionResult<MyProfileResponse>> GetMe() {
        string userId     = User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;
        var    findResult = await mediator.SendRequest(new FindUserByIdQuery(userId));
        if (!findResult.IsSuccess) return Problem(findResult.Errors);

        User user = findResult.Value!;

        long expires = Convert.ToInt64(
            User.FindFirst(JwtRegisteredClaimNames.Exp)!.Value
        );
        DateTimeOffset expiresOffset = DateTimeOffset.FromUnixTimeSeconds(expires);

        return Ok(new MyProfileResponse(
                user.Id.ToString(),
                user.Username,
                user.CreatedAt,
                user.LastLoginAt,
                user.Email,
                expiresOffset.UtcDateTime
            )
        );
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<ProfileResponse>> GetProfile(string username) {
        var findResult = await mediator.SendRequest(new FindUserByNameQuery(username));
        return findResult.Match(
            user => Ok(new ProfileResponse(user.Id.ToString(), user.Username, user.CreatedAt, user.LastLoginAt)),
            Problem
        );
    }

    [HttpPost("password")]
    public async Task<ActionResult> SetPassword([FromBody] ChangePasswordRequest request) {
        string email = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;

        var setPasswordResult = await mediator.SendRequest(
            new SetPasswordCommand(email, request.OldPassword, request.NewPassword)
        );

        return setPasswordResult.Match(ActionResult () => Ok(), Problem);
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult<DeleteResponse>> Delete(string username, [FromBody] DeleteRequest request) {
        string? email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        if (email is null)
            return Problem(Errors.User.InvalidEmail);

        var deleteResult = await mediator.SendRequest(new DeleteCommand(username, email, request.Password));

        return deleteResult.Match(() => Ok(new DeleteResponse(username, email)), Problem);
    }

}
