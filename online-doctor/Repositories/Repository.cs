using Dapper;
using online_doctor.Providers;
using System;
using System.Collections.Generic;
using System.Data;

namespace online_doctor.Repositories
{
    public abstract class Repository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public Repository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        protected void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null)
        {
            using (var connection = _connectionProvider.Open())
                connection.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
        }

        protected T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null)
        {
            using (var connection = _connectionProvider.Open())
                return (T)Convert.ChangeType(connection.ExecuteScalar(procedureName, param, commandType: CommandType.StoredProcedure), typeof(T));
        }

        protected IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using (var connection = _connectionProvider.Open())
                return connection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
        }
    }
}