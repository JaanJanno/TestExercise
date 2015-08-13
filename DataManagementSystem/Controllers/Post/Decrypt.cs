using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace DataManagementSystem.Controllers.Post.Decrypt
{

    /*
     * Class used for decrypting and encrypting strings in AES 128, ECB mode.
     */ 

    public class Decrypt
    {
        private static Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        private static byte[] aesKey;

        // AES key is retrieved from Web.config file.
        static Decrypt() 
        {
            string keyString = WebConfigurationManager.AppSettings["aesKey"];
            aesKey = encoding.GetBytes(keyString);
        }

        public static string EncryptString(string plainText)
        {
            byte[] encrypted;

            AesCryptoServiceProvider provider = createAesProvider();

            // Create a decrytor to perform the stream transform.
            ICryptoTransform encryptor = provider.CreateEncryptor(provider.Key, null); // null IV, because ECB mode

            // Create the streams used for encryption. 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return encoding.GetString(encrypted);

        }

        public static string DecryptString(string str)
        {
            byte[] encrypted = encoding.GetBytes(str);
            string decrypted;

            AesCryptoServiceProvider provider = createAesProvider();

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, null); // null IV, because ECB mode

            // Create the streams used for decryption. 
            using (MemoryStream msDecrypt = new MemoryStream(encrypted))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream 
                        // and place them in a string.
                        decrypted = srDecrypt.ReadToEnd();
                    }
                }
            }
            return decrypted;
        }

        // Creates a AesCryptoServiceProvider object in 128 bit ECB mode.
        private static AesCryptoServiceProvider createAesProvider()
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider();
            provider.Mode = CipherMode.ECB;
            provider.KeySize = 128;
            provider.Key = aesKey;
            return provider;
        }
    }
}