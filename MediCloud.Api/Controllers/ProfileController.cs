using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Contracts.Profile;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("profile")]
public class ProfileController(
    IUserRepository               userRepository,
    IRequestClient<DeleteCommand> deleteRequestClient
) : ApiController {

    [HttpGet("me")]
    public async Task<ActionResult<MyProfileResponse>> GetMe() {
        UserId userId;
        try {
            string id = User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            userId = UserId.Factory.Create(Guid.Parse(id));
        }
        catch { return Problem(Errors.Auth.InvalidCred); }

        if (await userRepository.FindByIdAsync(userId) is not { } user)
            return Problem(Errors.Auth.InvalidCred);

        long expires = Convert.ToInt64(
            User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value
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
        User? user = await userRepository.FindByUsernameAsync(username);
        return user is null
            ? Problem(Errors.User.UserNotFound)
            : Ok(new ProfileResponse(user.Id.ToString(), user.Username, user.CreatedAt, user.LastLoginAt));
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult<DeleteResponse>> Delete(string username, [FromBody] DeleteRequest request) {
        string? email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        if (email is null)
            return Problem(Errors.User.InvalidEmail);

        // TODO: Add Jti to blacklist
        var deleteResponse =
            await deleteRequestClient.GetResponse<Result>(
                new DeleteCommand(username, email, request.Password)
            );

        return deleteResponse.Message.Match(
            () => Ok(new DeleteResponse(username, email)),
            Problem
        );
    }

}
