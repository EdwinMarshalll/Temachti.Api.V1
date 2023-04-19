using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Temachti.Api.DTOs;

namespace Temachti.Api.Services;

public class HashService
{
    public DTOHashResult Hash(string plainText)
    {
        var salt = new byte[16];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
        }

        return Hash(plainText, salt);
    }

    public DTOHashResult Hash(string plainText, byte[] salt)
    {
        var keyDerivation = KeyDerivation.Pbkdf2(
            password: plainText,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32
        );

        var hash = Convert.ToBase64String(keyDerivation);

        return new DTOHashResult()
        {
            Hash = hash,
            Salt = salt
        };
    }
}