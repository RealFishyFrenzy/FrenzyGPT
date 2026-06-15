using System;
using System.Security.Cryptography;

public static class PasswordHasher
{
    public static string GenerateSalt()
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);

        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256,
            32
        );

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string salt, string storedHash)
    {
        string attemptedHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(attemptedHash),
            Convert.FromBase64String(storedHash)
        );
    }
}