using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Vnext.Intern.Utility
{
    public static class Utils
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static bool CheckIfFileIsBeingUsed(string fileName)
        {
            try
            {
                FileStream fs;
                fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch
            {
                return true;
            }
            return false;
        }

        public static string ForceXmlString(string input)
        {
            return input
                .Replace("&", "&#38;")
                .Replace("<", "&#60;")
                .Replace(">", "&#62;")
                .Replace("'", "&#39;")
                .Replace("\"", "&#34;")
                .Replace("\r\n", "<w:br/>");
        }
    }
}
