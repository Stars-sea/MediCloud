using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;

namespace MediCloud.Infrastructure.Authentication;

public class JwtTokenBlacklist(
    ICacheService cacheService
) : IJwtTokenBlacklist {

    public async Task<Result> BanTokenAsync(string jti, CancellationToken cancellationToken = default) {
        await cacheService.SetAsync($"jti.blacklist.{jti}", "", cancellationToken);
        return Result.Ok;
    }

    public async Task<Result> UnbanTokenAsync(string jti, CancellationToken cancellationToken = default) {
        await cacheService.RemoveAsync($"jti.blacklist.{jti}", cancellationToken);
        return Result.Ok;
    }

    public async Task<bool> IsTokenBanned(string jti, CancellationToken cancellationToken = default) {
        return await cacheService.GetAsync<string>($"jti.blacklist.{jti}", cancellationToken) is not null;
    }

}
