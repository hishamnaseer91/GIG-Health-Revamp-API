using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class Common
    {
        private static string key1 = "alert";
        public static string Encrypt(string toEncrypt)
        {
            bool useHashing = true;
            string securityKey = key1;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = securityKey;
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string cipherString)
        {
            bool useHashing = true;
            string securityKey = key1;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = securityKey;
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string GetRandomPassword()
        {
            //Guid guid = Guid.NewGuid();
            //Random random = new Random();
            //int psw = random.Next();
            //string password = psw.ToString();
            //password = password.Substring(0,4);

            ////string password = Guid.NewGuid().ToString().Substring(36 - length);
            //return password;

            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            string password = _rdm.Next(_min, _max).ToString();
            return password;

        }
        public static string GetRandomPasswordReset()
        {
            //Guid guid = Guid.NewGuid();
            //Random random = new Random();
            //int psw = random.Next();
            //string password = psw.ToString();
            //password = password.Substring(0,4);

            ////string password = Guid.NewGuid().ToString().Substring(36 - length);
            //return password;

            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            string password = _rdm.Next(_min, _max).ToString();
            return password;

        }
    }

    public class EmailTemplates
    {
        //public string GetAddComplainttemplate = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head> <title></title></head><body><div style =\"padding-left:10%; padding-right:15%;\"><h3>Dear Admin</h3><p style =\"font-size: 17px;\">A<strong> Complaint</strong> has been Registered by</p><p><strong>Civil ID: </strong>[CivilId]</p>        <p><strong> Policy No. </strong>[PolicyNo] </p>  <p> <strong>Network ID: </strong>[NetworkId]</p><p> <strong>MemberNo. </strong>[MemberNo]</p><p> <strong>Category: </strong>[Category] </p><p> <strong>Subject: </strong>[Subject] </p><p><strong>Description: </strong>[Decription]</p>   <p><b><i>Thanks</i></b></p></div></body></html>";


        public string GetAddComplainttemplate = @"<html>
                <head>
                    <title></title>
                </head>
                    <body>
                        <div>
                          <h4>Dear Admin</h4>
                          <p>A Complaint has been Registered by</p><br/>
                          <p><strong>Civil Id: </strong>[CivilId]</p><br/> 
                           <p><strong>Member Name: </strong>[Name]</p><br/> 
                          <p><strong> Policy No. </strong>[PolicyNo] </p><br/>
                          <p><strong> Member No. </strong>[MemberNo] </p><br/>  
                            <p> <strong>Complaint Category: </strong>[Category] </p><br/>
                          <p> <strong>Subject: </strong>[Subject] </p><br/>
                          <p><strong>Description: </strong>[Decription]</p><p>
                        <br/>
                        <b><i>Thanks</i></b></p></div>
                        </div>
                    </body>

                </html>";


    }


}