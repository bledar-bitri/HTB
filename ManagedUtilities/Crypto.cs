using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ManagedUtilities
{
    public class Crypto
    {   
        public string EncryptString(string stringToEncrypt, string strKey)
        {
            byte[] key = { };
            byte[] IV = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };
            try
            {
                key = Encoding.UTF8.GetBytes(strKey);
                DESCryptoServiceProvider desProvidr = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream csstreamdata = new CryptoStream(ms, desProvidr.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                csstreamdata.Write(inputByteArray, 0, inputByteArray.Length);
                csstreamdata.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecryptString(string stringToDecrypt, string strKey)
        {
            byte[] key = { };
            byte[] IV = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };
            stringToDecrypt = stringToDecrypt.Replace(" ", "+");
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            { 
                key = Encoding.UTF8.GetBytes(strKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TestCrypto(string testString, string key)
        {
            string strEncrypt = string.Empty;
            string strDecrypt = string.Empty;

            strEncrypt = EncryptString(testString, key);

            strDecrypt = DecryptString(strEncrypt, key);

            return strDecrypt;
        }

        public static void Main(String[] args)
        {
            //Console.Write(new Crypto().EncryptString("THIS IS A TEST", "M^K#Y$35"));
            Console.Write(new Crypto().DecryptString("uYWu53kY1J/+a9SxRFovL1IyPWAbu95MczJYsvtdvo5men4/a5M40TG/BO+kgbib7T3Kx3Xvy6zMs2ryoqPYU+9cYB0UVR09JK6zzv5QrSM/iofUlERgKyrg3HLzA42CNtkwBbJ+cQsE7AsoB9I2CnIMAPm0wcQu/yvK5GHjUoY+2dgDeG712kfQ65NTrCnADDLd5rQZyXwogLHvUSw9Cu9MFhpF2xn+bR51x0xV+KGP5smCkSZdO3RiBbnbBeu3L+ZNFRNmogKhEbFdSY6UJBLdKxj2viVJz84rMBZcj/g=", "M^K#Y$35"));
            Thread.Sleep(10000);
        }
    }
}
