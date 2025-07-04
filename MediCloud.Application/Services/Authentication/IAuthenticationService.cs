using ErrorOr;

namespace MediCloud.Application.Services.Authentication;

public interface IAuthenticationService {
    Task<ErrorOr<AuthenticationResult>> RegisterAsync(string username, string email, string password);
    
    Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password);
}
