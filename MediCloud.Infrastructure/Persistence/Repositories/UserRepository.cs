using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class UserRepository(
    MediCloudDbContext dbContext,
    IPasswordHasher    passwordHasher,
    IPasswordValidator passwordValidator
) : IUserRepository {

    public async Task<User?> FindByEmailAsync(string email) {
        string upperEmail = email.ToUpper();
        return await dbContext.Users.FirstOrDefaultAsync(u =>
            u.Email.ToUpper().Equals(upperEmail)
        );
    }

    public Task<bool> VerifyPasswordAsync(User user, string password) {
        if (string.IsNullOrEmpty(user.PasswordHash) && string.IsNullOrEmpty(password))
            return Task.FromResult(true);

        return Task.FromResult(passwordHasher.VerifyHashedPassword(user.PasswordHash, password));
    }

    public async Task<IList<Error>> CreateAsync(User user, string password) {
        IList<Error> errors = await passwordValidator.ValidateAsync(password);
        if (errors.Any()) return errors;

        string passwordHash = passwordHasher.HashPassword(password);
        user.PasswordHash = passwordHash;
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return [];
    }

    public async Task<IList<Error>> ChangePasswordAsync(User user, string oldPassword, string newPassword) {
        if (!await VerifyPasswordAsync(user, oldPassword))
            return [Errors.Auth.InvalidCred];

        IList<Error> errors = await passwordValidator.ValidateAsync(newPassword);
        if (errors.Any()) return errors;

        string passwordHash = passwordHasher.HashPassword(newPassword);
        user.PasswordHash = passwordHash;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return [];
    }

}
