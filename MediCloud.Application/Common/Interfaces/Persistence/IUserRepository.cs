using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface IUserRepository {

    Task<User?> FindByEmailAsync(string email);
    
    Task<bool> VerifyPasswordAsync(User user, string password);
    
    Task<IList<Error>> CreateAsync(User user, string password);
    
    Task<IList<Error>> ChangePasswordAsync(User user, string oldPassword, string newPassword);

}
