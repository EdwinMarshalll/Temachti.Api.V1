using System.Security.Cryptography;
using System.Text;

namespace Temachti.Api.Utils;

public class SecurityManager
{
    private const string _aesKey = "TWlTdXBlckNvbnRyYXNlbmFIZXJtb3Nh"; //MiSuperContrasenaHermosa
    private const string _aesIV = "TWlTdXBlckNvbnRyYXNlbmFIZXJtb3NhTWFzSGVybW9zYQ=="; //MiSuperContrasenaHermosaMasHermosa

    /// <summary>
    /// Encriptar un texto
    /// </summary>
    /// <param name="rawText">Cadena a ser encriptada</param>
    /// <exception cref="FormatException"></exception>
    public static string Encrypt(string rawText)
    {
        if (!string.IsNullOrEmpty(rawText))
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_aesKey);
                aes.IV = Convert.FromBase64String(_aesIV);
                byte[] encryptedBytes = EncryptStringToBytes_Aes(rawText, aes.Key, aes.IV);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Desencriptar un texto
    /// </summary>
    /// <param name="encryptedText">Cadena a ser desencriptada</param>
    /// <exception cref="FormatException"></exception>
    public static string Decrypt(string encryptedText)
    {
        if (!string.IsNullOrEmpty(encryptedText))
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_aesKey);
                aes.IV = Convert.FromBase64String(_aesIV);
                byte[] data = Convert.FromBase64String(encryptedText);
                return DecryptStringFromBytes_Aes(data, aes.Key, aes.IV);
            }
        }
        else
        {
            return string.Empty;
        }
    }

    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0) throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");

        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0) throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");

        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}