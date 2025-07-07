using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository {

    public Task<User?> FindByEmailAsync(string email) {
        throw new NotImplementedException();
    }

    public Task<bool> CheckPasswordAsync(User user, string password) {
        throw new NotImplementedException();
    }

    public Task<IList<Error>> CreateAsync(User user, string password) {
        throw new NotImplementedException();
    }

}
