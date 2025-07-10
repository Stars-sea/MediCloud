using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Handlers;

public class RefreshCommandHandler(
    IUserRepository   userRepository,
    IJwtTokenManager  jwtTokenManager
) : IRequestHandler<RefreshCommand, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(RefreshCommand request) {
        (string email, string jti, string expiresStamp) = request;

        if (await userRepository.FindByEmailAsync(email) is not { } user)
            return Errors.User.UserNotFound;
        
        DateTime expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiresStamp)).UtcDateTime;

        Result<JwtGenerateResult> generateResult = jwtTokenManager.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;
        
        await jwtTokenManager.BanTokenAsync(jti, expires);

        (string token, DateTime newExpires) = generateResult.Value!;
        return new AuthenticationResult(user, token, newExpires);
    }

}
