using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using Org.BouncyCastle.Crypto.Generators;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class UserRepository : Repository
    {
        public UserRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<User> GetAllUsers()
        {
            return ReturnList<User>("GetAllUsers", null).ToList<User>();
        }

        public User GetUserByLogin(string login)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserLogin", login);
            return ReturnList<User>("GetUserByLogin", param).FirstOrDefault<User>();
        }

        public void RegistrationUser(User user)
        {
            DynamicParameters param = new DynamicParameters();
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            string key = EncryptoUtils.EncryptoUtils.RandomString(50);
            //string EncryptedFIO = SimpleAES.AES256.Encrypt(user.FIO, key);
            //string EncryprtedEmail = SimpleAES.AES256.Encrypt(user.Email, key);

            string EncryptedFIO = user.FIO;
            string EncryprtedEmail = user.Email;

            param.Add("@FIO", EncryptedFIO);
            param.Add("@Email", EncryprtedEmail);
            param.Add("@Login", user.Login);
            param.Add("@Birthday", user.Birthday);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword, salt);

            param.Add("@UserPassword", hashedPassword);
            //param.Add("@SymmetricKey", key);
            ExecuteWithoutReturn("RegistrationUser", param);
        }
    }
}
