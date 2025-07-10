using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {

    Result<JwtGenerateResult> GenerateToken(User user);

}
