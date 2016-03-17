using System;
using System.Security.Cryptography;
using System.Text;
using Plot.Attributes;

namespace Plot.Sample
{
    [Serializable]
    public class Password
    {
        private static Random Random = new Random();

        public Password()
        {
            Created = DateTime.UtcNow;
        }

        public virtual string Id { get; set; }

        public virtual string Hash { get; set; }

        public virtual string Salt { get; set; }

        public virtual DateTime Created { get; set; }

        [Relationship(Relationships.AuthenticatesWith, Reverse = true)]
        public virtual User User { get; set; }

        public virtual bool Verify(string password)
        {
            var hash = GenerateHash(Encoding.UTF8.GetBytes(password), Convert.FromBase64String(Salt));
            return Compare(Convert.FromBase64String(Hash), hash);
        }

        public static Password Create(string password)
        {
            var salt = GenerateSalt();
            var hash = GenerateHash(Encoding.UTF8.GetBytes(password), salt);
            return new Password {Hash = Convert.ToBase64String(hash), Salt = Convert.ToBase64String(salt)};
        }

        private static byte[] GenerateSalt()
        {
            var crypto = new RNGCryptoServiceProvider();
            var bytes = new byte[256];
            crypto.GetBytes(bytes);
            return bytes;
        }

        private static byte[] GenerateHash(byte[] password, byte[] salt)
        {
            var algorithm = new SHA256Managed();
            byte[] plainTextWithSaltBytes = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++)
            {
                plainTextWithSaltBytes[i] = password[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private static bool Compare(byte[] stored, byte[] password)
        {
            if (stored.Length != password.Length)
            {
                return false;
            }
            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static string Generate(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[Random.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
