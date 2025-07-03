using MediCloud.Application.Services.Authentication;
using MediCloud.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(
    IAuthenticationService service
) : ControllerBase {
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request) {
        AuthenticationResult   result   = await service.LoginAsync(request.Email, request.Password);
        AuthenticationResponse response = new(Guid.NewGuid(), result.Email, result.Token);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request) {
        AuthenticationResult   result   = await service.RegisterAsync(request.Username, request.Email, request.Password);
        AuthenticationResponse response = new(Guid.NewGuid(), result.Email, result.Token);
        return Ok(response);
    }
}
