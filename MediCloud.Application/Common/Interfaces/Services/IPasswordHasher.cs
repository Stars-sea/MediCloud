namespace MediCloud.Application.Common.Interfaces.Services;

public interface IPasswordHasher {

    string HashPassword(string password);
    
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);

}
