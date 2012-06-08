using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QRTracker
{
    public static class OtherFunctions
    {
        public static string StripDomain(string username)
        {
            int pos = username.IndexOf('\\');
            return pos != -1 ? username.Substring(pos + 1) : username;
        }

        public static string GetFullName(string strLogin)
        {
            string str = "";
            string strDomain;
            string strName;

            // Parse the string to check if domain name is present.
            int idx = strLogin.IndexOf('\\');
            if (idx == -1)
            {
                idx = strLogin.IndexOf('@');
            }

            if (idx != -1)
            {
                strDomain = strLogin.Substring(0, idx);
                strName = strLogin.Substring(idx + 1);
            }
            else
            {
                strDomain = Environment.MachineName;
                strName = strLogin;
            }

            DirectoryEntry obDirEntry = null;
            try
            {
                obDirEntry = new DirectoryEntry("WinNT://" + strDomain + "/" + strName);
                System.DirectoryServices.PropertyCollection coll = obDirEntry.Properties;
                object obVal = coll["FullName"].Value;
                str = obVal.ToString();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool verifyMd5Hash(string input)
        {
            var parts = input.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            
            string hash = string.Empty;
            string text = string.Empty;
            try
            {
                hash = parts[parts.Count() - 1];
                int startIndex = input.IndexOf(hash);
                text = input.Substring(0, startIndex);
            }
            catch (Exception)
            {
                
                throw;
            }
            
            string hashOfInput = getMd5Hash(text);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                if (string.IsNullOrEmpty(hash))
                    return false;
                else
                {
                  return true;  
                }
                
            }
            else
            {
                return false;
            }
        }


        
    }
}