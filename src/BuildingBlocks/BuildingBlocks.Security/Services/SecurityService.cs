using System.Security.Cryptography;
using System.Text;

namespace BuildingBlocks.Security.Services;

public static class SecurityService
{
    public static string GetSha256Hash(string input)
    {
        using var hashAlgorithm = SHA256.Create();
        byte[] byteValue = Encoding.UTF8.GetBytes(input);
        byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
        return Convert.ToBase64String(byteHash);
    }

    public static Guid CreateCryptographicallySecureGuid()
    {
        var rand = RandomNumberGenerator.Create();

        byte[] bytes = new byte[16];
        rand.GetBytes(bytes);

        return new Guid(bytes);
    }
}