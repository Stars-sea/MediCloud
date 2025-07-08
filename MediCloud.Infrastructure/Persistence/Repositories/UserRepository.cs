using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Common.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MediCloud.Infrastructure.Persistence.Repositories;

public class UserRepository(
    MediCloudDbContext dbContext,
    IPasswordHasher    passwordHasher,
    IPasswordValidator passwordValidator
) : IUserRepository {

    public async Task<User?> FindByIdAsync(UserId id) { return await dbContext.Users.FindAsync(id); }

    public async Task<User?> FindByEmailAsync(string email) {
        string upperEmail = email.ToUpper();
        return await dbContext.Users.FirstOrDefaultAsync(u =>
            u.Email.ToUpper().Equals(upperEmail)
        );
    }

    public async Task<User?> FindByUsernameAsync(string username) {
        string upperUsername = username.ToUpper();
        return await dbContext.Users.FirstOrDefaultAsync(u =>
            u.Username.ToUpper().Equals(upperUsername)
        );
    }

    public Task<bool> VerifyPasswordAsync(User user, string password) {
        if (string.IsNullOrEmpty(user.PasswordHash) && string.IsNullOrEmpty(password))
            return Task.FromResult(true);

        return Task.FromResult(passwordHasher.VerifyHashedPassword(user.PasswordHash, password));
    }

    public async Task<Result> CreateAsync(User user, string password) {
        Result result = await passwordValidator.ValidateAsync(password);
        if (!result.IsSuccess) return result;

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
        Result result = await passwordValidator.ValidateAsync(password);
        if (!result.IsSuccess) return result;

        user.PasswordHash = passwordHasher.HashPassword(password);

        return await UpdateAsync(user);
    }

}
