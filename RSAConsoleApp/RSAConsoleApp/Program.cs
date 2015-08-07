using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSAConsoleApp
{
    class Program
    {
        static void Main()
        {
            using (var encrypterRSA = new RSACryptoServiceProvider())
            using (var decrypterRSA = new RSACryptoServiceProvider())
            {
                // Write to string the Keys
                var privXmlString = encrypterRSA.ToXmlString(true);
                var publicXmlString = encrypterRSA.ToXmlString(false);

                // Encrypt the message
                var byteDataEncode = Encoding.UTF8.GetBytes("Message for you");
                var encryptMsg = encrypterRSA.Encrypt(byteDataEncode, false);

                // Decrypt
                decrypterRSA.FromXmlString(privXmlString);
                var decryptMsg = decrypterRSA.Decrypt(encryptMsg, false);

                // Show the message
                var originalMsg = Encoding.UTF8.GetString(decryptMsg);
                Console.WriteLine(originalMsg);
            }
        }
    }
}
