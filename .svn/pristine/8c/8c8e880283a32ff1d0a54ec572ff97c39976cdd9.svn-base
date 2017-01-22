using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ManagedUtilities
{
  public class Security
    {

      public string Encrypt(string strText, string Key)
      {
          try
          {
              ASCIIEncoding textConverter = new ASCIIEncoding();
              DESCryptoServiceProvider rc2CSP = new DESCryptoServiceProvider();
              byte[] encrypted;
              byte[] toEncrypt;
              byte[] key;
               
              PasswordDeriveBytes PassDerByt = new PasswordDeriveBytes(Key, null);

              rc2CSP.GenerateIV();
              key = PassDerByt.CryptDeriveKey("rc2", PassDerByt.HashName.ToString(), 128, rc2CSP.IV);
              ICryptoTransform encryptor = rc2CSP.CreateEncryptor(key, rc2CSP.IV);
              MemoryStream msEncrypt = new MemoryStream();
              CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
              msEncrypt.Seek(0, System.IO.SeekOrigin.Begin);
              toEncrypt = UnicodeEncoding.UTF32.GetBytes(strText); //Write all data to the crypto stream and flush it. 
              csEncrypt.Seek(0, System.IO.SeekOrigin.Begin);
              csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
              csEncrypt.FlushFinalBlock();            //Get encrypted array of bytes.            encrypted = msEncrypt.ToArray();
              encrypted = msEncrypt.ToArray();

              return Convert.ToBase64String(encrypted);
          }

          catch (CryptographicException eYpt)
          {
              return eYpt.Message.ToString();
          }

          catch (Exception ee)
          {
              return ee.Message.ToString();
          }
      }

      public string Decrypt(string aInfo, string Key)
      {
          try
          {
              DESCryptoServiceProvider rc2CSP = new DESCryptoServiceProvider();
              byte[] fromEncrypt;
              byte[] key;
              byte[] encrypted;

              PasswordDeriveBytes PassDerByt = new PasswordDeriveBytes(Key, null);

              rc2CSP.GenerateIV();
              key = PassDerByt.CryptDeriveKey("rc2", PassDerByt.HashName.ToString(), 128, rc2CSP.IV);
              encrypted = (Convert.FromBase64String(aInfo));  
              ICryptoTransform decryptor = rc2CSP.CreateDecryptor(key, rc2CSP.IV);
              MemoryStream msDecrypt = new MemoryStream(encrypted);
              msDecrypt.Seek(0, System.IO.SeekOrigin.Begin);
              CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
              fromEncrypt = new byte[msDecrypt.Length];  //Read the data out of the crypto stream. 
              csDecrypt.Seek(0, System.IO.SeekOrigin.Begin);
              csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);   //Convert the byte array back into a string.
              csDecrypt.Flush();

              return UnicodeEncoding.UTF32.GetString(fromEncrypt);
          }

          catch (CryptographicException eYpt)
          {
              return eYpt.Message.ToString();
          }

          catch (Exception ee)
          {
              return ee.Message.ToString(); ;
          }
      }

    }
}
