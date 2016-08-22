using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Encryption.RSA
{
    class RSACrypto
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public static byte[] DecryptBytes(byte[] data, byte[] pvk)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(pvk);
            var decryptedByte = rsa.Decrypt(data, false);
            return decryptedByte;
        }
        public static byte[] EncryptBytes(byte[] data, byte[] puk)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(puk);
            var encryptedByteArray = rsa.Encrypt(data, false).ToArray();
            return encryptedByteArray;
        }

        public static string DecryptString(string data, byte[] pvk)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.ImportCspBlob(pvk);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public static byte[] EncryptString(string data, byte[] puk)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(puk);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();

            return encryptedByteArray;
        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
