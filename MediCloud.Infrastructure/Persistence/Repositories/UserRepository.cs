using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class UserRepository(
    MediCloudDbContext dbContext,
    IPasswordHasher    passwordHasher
) : IUserRepository {

    public async Task<User?> FindByIdAsync(UserId id) { return await dbContext.Users.FindAsync(id); }

    public Task<User?> FindByEmailAsync(string email) {
        string upperEmail = email.ToUpper();
        return dbContext.Users.FirstOrDefaultAsync(u =>
            u.Email.ToUpper().Equals(upperEmail)
        );
    }

    public Task<User?> FindByUsernameAsync(string username) {
        string upperUsername = username.ToUpper();
        return dbContext.Users.FirstOrDefaultAsync(u =>
            u.Username.ToUpper().Equals(upperUsername)
        );
    }

    public Task<bool> VerifyPasswordAsync(User user, string password) {
        if (string.IsNullOrEmpty(user.PasswordHash) && string.IsNullOrEmpty(password))
            return Task.FromResult(true);

        return Task.FromResult(passwordHasher.VerifyHashedPassword(user.PasswordHash, password));
    }

    public async Task<Result> CreateAsync(User user, string password) {
        user.PasswordHash = passwordHasher.HashPassword(password);
        await dbContext.Users.AddAsync(user);
        
        try { await dbContext.SaveChangesAsync(); }
        catch (DbUpdateException) { return Errors.User.FailedToUpdate; }

        return Result.Ok;
    }

    public async Task<Result> UpdateAsync(User user) {
        dbContext.Users.Update(user);
        try { await dbContext.SaveChangesAsync(); }
        catch (DbUpdateException) { return Errors.User.FailedToUpdate; }

        return Result.Ok;
    }

    public async Task<Result> DeleteAsync(User user) {
        dbContext.Users.Remove(user);
        try { await dbContext.SaveChangesAsync(); }
        catch (DbUpdateException) { return Errors.User.FailedToUpdate; }

        return Result.Ok;
    }

    public async Task<Result> SetPasswordAsync(User user, string password) { 
        user.PasswordHash = passwordHasher.HashPassword(password);
        user.UpdateSecurityStamp();
        return await UpdateAsync(user);
    }
}
