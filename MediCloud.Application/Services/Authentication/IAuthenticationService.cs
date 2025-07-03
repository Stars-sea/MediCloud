namespace MediCloud.Application.Services.Authentication;

public interface IAuthenticationService {
    Task<AuthenticationResult> RegisterAsync(string username, string email, string password);
    
    Task<AuthenticationResult> LoginAsync(string email, string password);
}
