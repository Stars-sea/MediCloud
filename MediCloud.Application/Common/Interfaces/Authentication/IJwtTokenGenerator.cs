namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {
    string GenerateToken(string username, string userId, string email);
}
