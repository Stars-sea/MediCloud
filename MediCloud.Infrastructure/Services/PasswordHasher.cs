using System.Security.Cryptography;
using MediCloud.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MediCloud.Infrastructure.Services;

// https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs

public class PasswordHasher : IPasswordHasher {

    private readonly RandomNumberGenerator _rng           = RandomNumberGenerator.Create();
    private const    KeyDerivationPrf      Prf            = KeyDerivationPrf.HMACSHA512;
    private const    int                   IterationCount = 100_000;
    private const    int                   SaltSize       = 128 / 8;

    public string HashPassword(string password) {
        if (string.IsNullOrEmpty(password))
            throw new InvalidOperationException("Password cannot be null or empty.");

        byte[] salt = new byte[SaltSize];
        _rng.GetBytes(salt);
        
        byte[] subkey = KeyDerivation.Pbkdf2(
            password, salt, Prf,
            IterationCount, 256 / 8
        );

        byte[] output = new byte[salt.Length + subkey.Length];
        Buffer.BlockCopy(salt, 0, output, 0, salt.Length);
        Buffer.BlockCopy(subkey, 0, output, SaltSize, subkey.Length);

        return Convert.ToBase64String(output);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword) {
        if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            throw new InvalidOperationException("Hashed password and provided password cannot be null or empty.");

        byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
        if (decodedHashedPassword.Length == 0) return false;

        try { return VerifyHashedPassword(decodedHashedPassword, providedPassword); }
        catch { return false; }
    }

    private static bool VerifyHashedPassword(byte[] hashedPassword, string providedPassword) {
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPassword, 0, salt, 0, SaltSize);

        int    subkeyLength   = hashedPassword.Length - SaltSize;
        byte[] expectedSubkey = new byte[subkeyLength];
        Buffer.BlockCopy(
            hashedPassword, SaltSize,
            expectedSubkey, 0, subkeyLength
        );

        byte[] actualSubkey = KeyDerivation.Pbkdf2(
            providedPassword, salt, Prf,
            IterationCount, subkeyLength
        );
        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }

}
