using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenManager {
    
    public const string SecurityStampClaim = "security_stamp";

    Result<JwtGenerateResult> GenerateToken(User user);

    Task<Result> BanTokenAsync(string jti, CancellationToken cancellationToken = default);
    
    Task<Result> UnbanTokenAsync(string jti, CancellationToken cancellationToken = default);
    
    Task<bool> IsTokenBanned(string jti, CancellationToken cancellationToken = default);

}
