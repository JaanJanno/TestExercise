using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace ControlPanel.Controllers.Post
{
    public class Encrypt
    {
        private static Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        private static byte[] aesKey;

        static Encrypt() 
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