/*
 * RSA Assymetric Demo by SevenString
 * 
 * The keys are stored either on memory or from file. 
 * There is an interface to select between the two storage.
 * 
 * License: CC BY 2.0
 * */

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



//-#-----#-----------------
//-##---##---##---#-#----#-
//-#-#-#-#--#--#--#-##---#-
//-#--#--#-#----#-#-#-#--#-
//-#-----#-######-#-#--#-#-
//-#-----#-#----#-#-#---##-
//-#-----#-#----#-#-#----#-
//-------------------------

        static void Main()
        {
            IAssymetricKeyStorage assymetricStorage;

            // Comment either one of this lines to test for memory or xml storage
            //assymetricStorage = new AssymetricKeyStoreInMemory();
            assymetricStorage = new AssymetricKeyFromXmlFile("c:/privkey.xml", "c:/pubkey.xml");

            Console.WriteLine(assymetricStorage.GetType().ToString()); // to see if we are storing it on memory or xml file

            using (var encrypterRSA = new RSACryptoServiceProvider())
            using (var decrypterRSA = new RSACryptoServiceProvider())
            {
                // Write to string the Keys
                var privXmlString = encrypterRSA.ToXmlString(true);
                var publicXmlString = encrypterRSA.ToXmlString(false);

                assymetricStorage.StorePrivateKey(privXmlString);
                assymetricStorage.StorePublicKey(publicXmlString);

                // Encrypt the message
                var byteDataEncode = Encoding.UTF8.GetBytes("Message for you");
                var encryptMsg = encrypterRSA.Encrypt(byteDataEncode, false);

                // Decrypt
                decrypterRSA.FromXmlString(assymetricStorage.GetPrivateKey());
                var decryptMsg = decrypterRSA.Decrypt(encryptMsg, false);

                // Show the message
                var originalMsg = Encoding.UTF8.GetString(decryptMsg);
                Console.WriteLine(originalMsg);
                Console.ReadKey();
            }
        }




//-###--------------------------------------------------------
//--#--#----#-#####-######-#####--######---##----####--######-
//--#--##---#---#---#------#----#-#-------#--#--#----#-#------
//--#--#-#--#---#---#####--#----#-#####--#----#-#------#####--
//--#--#--#-#---#---#------#####--#------######-#------#------
//--#--#---##---#---#------#---#--#------#----#-#----#-#------
//-###-#----#---#---######-#----#-#------#----#--####--######-
//------------------------------------------------------------



        public interface IAssymetricKeyStorage
        {
            void StorePublicKey(string key);
            void StorePrivateKey(string key);

            string GetPrivateKey();
        }




//-#######-------------------------#-----#-----------------------------------
//-#-------#####---####--#----#----##---##-######-#----#--####--#####--#---#-
//-#-------#----#-#----#-##--##----#-#-#-#-#------##--##-#----#-#----#--#-#--
//-#####---#----#-#----#-#-##-#----#--#--#-#####--#-##-#-#----#-#----#---#---
//-#-------#####--#----#-#----#----#-----#-#------#----#-#----#-#####----#---
//-#-------#---#--#----#-#----#----#-----#-#------#----#-#----#-#---#----#---
//-#-------#----#--####--#----#----#-----#-######-#----#--####--#----#---#---
//---------------------------------------------------------------------------


        public class AssymetricKeyStoreInMemory : IAssymetricKeyStorage
        {
            string mPublicKey;
            string mPrivateKey;

            public void StorePublicKey(string key)
            {
                mPublicKey = key;
            }

            public void StorePrivateKey(string key)
            {
                mPrivateKey = key;
            }

            public string GetPrivateKey()
            {
                return mPrivateKey;
            }
        }



//-#######-------------------------#######-----------------
//-#-------#####---####--#----#----#-------#-#------######-
//-#-------#----#-#----#-##--##----#-------#-#------#------
//-#####---#----#-#----#-#-##-#----#####---#-#------#####--
//-#-------#####--#----#-#----#----#-------#-#------#------
//-#-------#---#--#----#-#----#----#-------#-#------#------
//-#-------#----#--####--#----#----#-------#-######-######-
//---------------------------------------------------------



        public class AssymetricKeyFromXmlFile : IAssymetricKeyStorage
        {
            string mPrivKeyFile;
            string mpublicKeyFile;


            public AssymetricKeyFromXmlFile(string privKeyFile,string publicKeyFile)
            {
                mPrivKeyFile = privKeyFile;
                mpublicKeyFile = publicKeyFile;
            }


            public void StorePublicKey(string key)
            {
                File.WriteAllText(mpublicKeyFile, key);
            }

            public void StorePrivateKey(string key)
            {
                File.WriteAllText(mPrivKeyFile, key);
            }

            public string GetPrivateKey()
            {
                return File.ReadAllText(mPrivKeyFile).Trim();
            }
        }



    }
}
