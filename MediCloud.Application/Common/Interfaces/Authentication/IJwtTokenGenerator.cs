using MediCloud.Domain.Entities;

namespace MediCloud.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {

    string GenerateToken(User user);

}
