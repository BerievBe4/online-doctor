using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace online_doctor.Providers
{
    public class MySqlConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString;

        public MySqlConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionStringToMySql");
        }

        public IDbConnection Open()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
