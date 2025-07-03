using MediCloud.Application.Common.Interfaces.Authentication;

namespace MediCloud.Application.Services.Authentication;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator
) : IAuthenticationService {
    public Task<AuthenticationResult> RegisterAsync(string username, string email, string password) {
        // TODO
        
        Guid id = Guid.NewGuid(); // Temp
        
        string token = jwtTokenGenerator.GenerateToken(username, id.ToString(), email);
        return Task.FromResult(new AuthenticationResult(id, email, token));
    }

    public Task<AuthenticationResult> LoginAsync(string email, string password) {
        string token = jwtTokenGenerator.GenerateToken("", "", email);
        return Task.FromResult(new AuthenticationResult(Guid.NewGuid(), email, token));
    }
}
