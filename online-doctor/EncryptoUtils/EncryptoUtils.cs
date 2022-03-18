using System;
using System.Text;

namespace online_doctor.EncryptoUtils
{
    public class EncryptoUtils
    {
        private static Random _random = new Random(Environment.TickCount);
        private static string _alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string RandomString(int length)
        {
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(_alphabet[_random.Next(_alphabet.Length)]);

            return builder.ToString();
        }
    }
}
