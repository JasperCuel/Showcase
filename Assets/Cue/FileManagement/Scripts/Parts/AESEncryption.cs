using System;
using System.Security.Cryptography;
using System.Text;

namespace Cue.FileManagement
{
    internal static class AESEncryption
    {
        private const string Secret = "SugarVita";
        private static readonly char[] padding = { '=' };

        private static readonly byte[] ivBytes = new byte[16];
        private static readonly byte[] keyBytes = new byte[16];

        private static void GenerateIVBytes()
        {
            Random rnd = new();
            rnd.NextBytes(ivBytes);
        }

        private static void GenerateKeyBytes()
        {
            int sum = 0;
            foreach (char curChar in Secret)
                sum += curChar;
            Random rnd = new(sum);
            rnd.NextBytes(keyBytes);
        }

        internal static string Encrypt(string data)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(keyBytes, ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer).TrimEnd(padding).Replace('+', '-').Replace('/', '_');

            return ivString + encryptedString;
        }

        internal static string Decrypt(this string text)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            int endOfIVBytes = ivBytes.Length / 2;

            string ivString = text.Substring(0, endOfIVBytes);
            byte[] extractedIVBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = text.Substring(endOfIVBytes);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(keyBytes, extractedIVBytes);

            string incoming = encryptedString.Replace('_', '/').Replace('-', '+');
            switch (encryptedString.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            byte[] inputBuffer = Convert.FromBase64String(incoming);
            byte[] outpubBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string decryptedString = Encoding.Unicode.GetString(outpubBuffer);
            return decryptedString;
        }
    }
}