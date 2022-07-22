using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Crypto
{
    public class CryptoService : ICryptoService
    {
        private readonly string keyCrypt;

        public CryptoService()
        {
            keyCrypt = "key";
        }

        public string Decrypt(string str)
        {
            CryptoStream cs = null;
            StreamReader sr = null;
            try
            {
                cs = Decrypt(Convert.FromBase64String(str), keyCrypt);
                sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (CryptographicException)
            {
                return null;
            }
            finally
            {
                if (cs != null)
                {
                    cs.Close();
                    cs.Dispose();
                }

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        public string Encrypt(string str)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
        }

        /// <summary>
        /// Encrypt byte array
        /// </summary>
        /// <param name="value">Byte array to encrypt</param>
        /// <param name="key">Key to encrypt</param>
        /// <returns>Crypted byte array</returns>
        /// <developer>Serhii Rozniuk</developer>
        /// <date>20.08.2021</date>
        private byte[] Encrypt(byte[] value, string key)
        {
            SymmetricAlgorithm alg = Rijndael.Create();
            ICryptoTransform crypto = alg.CreateEncryptor(new PasswordDeriveBytes(key, null).GetBytes(16), new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write);

            cs.Write(value, 0, value.Length);
            cs.FlushFinalBlock();

            byte[] res = ms.ToArray();

            ms.Close();
            ms.Dispose();

            cs.Close();
            cs.Dispose();

            return res;
        }

        /// <summary>
        /// Decrypt byte array
        /// </summary>
        /// <param name="value">Byte array to decrypt</param>
        /// <param name="key">Key to decrypt</param>
        /// <returns>Derypted byte array</returns>
        /// <developer>Serhii Rozniuk</developer>
        /// <date>20.08.2021</date>
        private CryptoStream Decrypt(byte[] value, string key)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(new PasswordDeriveBytes(key, null).GetBytes(16), new byte[16]);
            MemoryStream ms = new MemoryStream(value);

            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}
