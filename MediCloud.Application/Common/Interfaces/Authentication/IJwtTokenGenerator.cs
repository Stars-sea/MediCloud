using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {

    string GenerateToken(User user);

}
