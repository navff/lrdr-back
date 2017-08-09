using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace API.Common
{
    public static class Security
    {
        /// <summary>
        /// Генерит случайный токен по алгоритму SHA256
        /// </summary>
        /// <returns>Хэш-строка, длиной 64 символа</returns>
        public static string GenerateToken()
        {
            var guid = Guid.NewGuid().ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(guid);
            var hash =  SHA256.Create().ComputeHash(bytes, 0, 32);
            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
    }
}