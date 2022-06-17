using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    public  class HashComputer
    {
        public static string ComputeRequestHash(IDictionary<string, string> pars)
        {
            string hash = string.Empty;

            foreach (KeyValuePair<string, string> kvp in pars)
            {
                hash += ComputeMD5Hash(kvp.Key + kvp.Value).Substring(0, 16).ToLower();
            }

            return ComputeMD5Hash(hash).Substring(0, 16).ToLower();
        }

        private static string ComputeMD5Hash(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Создание байтового массива
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Преобразование byte array => hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
