using System;
using System.IO;
using System.Security.Cryptography;

//https://msdn.microsoft.com/en-us/library/system.security.cryptography.aes(v=vs.110).aspx

namespace Encryptor
{
    public class AESCrypto
    {
        public static byte[] EncryptStringToBytes(string text, byte[] key, byte[] IV)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key
                    , aesAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt
                        , encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(
                            csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }


                // Return the encrypted bytes from the memory stream.
                return encrypted;
            }
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            // Declare the Aes object
            // used to decrypt the data.
            Aes aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Create an Aes object
                // with the specified key and IV.
                aesAlg = Aes.Create();
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);
               
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                do
                {
                    plaintext = srDecrypt.ReadLine();
                } while (!srDecrypt.EndOfStream);
                
            }
            finally
            {
                // Clean things up.

                // Close the streams.
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                // Clear the Aes object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;

        }
    }
}