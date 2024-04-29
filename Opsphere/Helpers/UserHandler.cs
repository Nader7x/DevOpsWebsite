namespace Opsphere.Helpers;
using System;
using System.Security.Cryptography;
using System.Text;

public static class UserHandler
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Compute hash of provided password and compare it with the stored hash
        string hashedProvidedPassword = HashPassword(password);
        return hashedProvidedPassword.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
    }


}