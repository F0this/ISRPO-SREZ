using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.service
{
    public class Secure
    {
        /// <summary>
        /// Шифрование алгоритмом SHA-256
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string Sha256(string inputString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
