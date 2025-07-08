using MediCloud.Domain.Common.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Persistence;

public interface IUserRepository {
    
    Task<User?> FindByIdAsync(UserId id);

    Task<User?> FindByEmailAsync(string email);
    
    Task<User?> FindByUsernameAsync(string username);
    
    Task<bool> VerifyPasswordAsync(User user, string password);
    
    Task<Result> CreateAsync(User user, string password);
    
    Task<Result> UpdateAsync(User user);
    
    Task<Result> DeleteAsync(User user);
    
    Task<Result> SetPasswordAsync(User user, string password);

}
