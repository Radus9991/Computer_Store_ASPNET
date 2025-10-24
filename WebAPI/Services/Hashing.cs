using ApplicationCore.Services;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Services
{
    public class Hashing : IPasswordAlgorithm
    {
        public bool ComparePasswordHash(string password, byte[] hash, byte[] salt)
        {
            HMACSHA512 hmac = new HMACSHA512(salt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(hash);
        }

        public void CreatePassword(string password, out byte[] hash, out byte[] salt)
        {
            HMACSHA512 hmac = new HMACSHA512();

            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
