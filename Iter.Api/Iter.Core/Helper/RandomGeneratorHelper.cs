using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Core.Helper
{
    public class RandomGeneratorHelper
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        private static readonly Random random = new Random();

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

        public static string GenerateReservationNumber()
        {
            const string prefix = "RESN";
            int randomNumber = random.Next(1000, 9999); 
            return $"{prefix}{randomNumber}";
        }
    }
}
