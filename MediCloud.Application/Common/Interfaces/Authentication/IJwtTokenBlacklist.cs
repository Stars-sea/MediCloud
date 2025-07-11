using MediCloud.Application.Common.Contracts;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenBlacklist {
    
    public const string SecurityStampClaim = "security_stamp";

    Task<Result> BanTokenAsync(
        string            jti,
        DateTimeOffset?   absExpiration     = null,
        CancellationToken cancellationToken = default
    );
    
    Task<Result> UnbanTokenAsync(string jti, CancellationToken cancellationToken = default);
    
    Task<bool> IsTokenBanned(string jti, CancellationToken cancellationToken = default);

}
