using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Core.Helper
{
    public class PasswordGeneratorHelper
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

        public static string GenerateRandomPassword(int length = 8)
        {
            if (length <= 0)
                throw new ArgumentException();
            
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteBuffer = new byte[length];
                rng.GetBytes(byteBuffer);
                var charBuffer = new char[length];
                for (int i = 0; i < length; i++)
                {
                    charBuffer[i] = AllowedChars[byteBuffer[i] % AllowedChars.Length];
                }
                return new string(charBuffer);
            }
        }
    }
}
