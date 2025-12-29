using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Domain.Common;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {

    Result<JwtGenerateResult> GenerateToken(User user);

}
