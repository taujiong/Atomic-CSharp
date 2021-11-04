using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class AtomicStringExtensions
    {
        public static string Md5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashedBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashedByte in hashedBytes)
                {
                    sb.Append(hashedByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}