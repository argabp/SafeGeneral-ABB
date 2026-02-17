using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ABB.Infrastructure.Data
{
    public class ABBDbConnectionCSM : IDbConnectionCSM, IDisposable
    {
        public ABBDbConnectionCSM(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ABBConnectionCSM");
            var connection = new SqlConnection(connectionString);
            Connection = connection;
        }

        private DbConnection Connection { get; }

        public async Task ExecuteNonQueryProc(string query, object param = null)
        {
            await Connection.ExecuteAsync(query, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<object> ExecuteScalarProc(string query, object param = null)
        {
            return await Connection.ExecuteScalarAsync(query, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> QueryProc(string query, object param = null)
        {
            return await Connection.QueryAsync(query, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryProc<T>(string query, object param = null)
        {
            return await Connection.QueryAsync<T>(query, param, commandType: CommandType.StoredProcedure, commandTimeout:1000);
        }

        public async Task<dynamic> QueryFirstProc(string query, object param = null)
        {
            return await Connection.QueryFirstOrDefaultAsync(query, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> QueryFirstProc<T>(string query, object param = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<T>(query, param, commandType: CommandType.StoredProcedure);
        }


        public async Task ExecuteNonQuery(string query, object param = null)
        {
            await Connection.ExecuteAsync(query, GetParameters(param));
        }

        private object GetParameters(object param)
        {
            if (DynamicHelper.IsDictionary(param))
            {
                var dictionaries = param as Dictionary<string, object>;
                var parameters = new DynamicParameters();
                foreach (var parameter in dictionaries)
                {
                    parameters.Add(parameter.Key, parameter.Value);
                }

                return parameters;
            }

            return param;
        }

        public async Task<object> ExecuteScalar(string query, object param = null)
        {
            return await Connection.ExecuteScalarAsync(query, param);
        }

        public async Task<IEnumerable<dynamic>> Query(string query, object param = null)
        {
            return await Connection.QueryAsync(query, param);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, object param = null)
        {
            return await Connection.QueryAsync<T>(query, param, commandTimeout:1000);
        }

        public async Task<dynamic> QueryFirst(string query, object param = null)
        {
            return await Connection.QueryFirstOrDefaultAsync(query, param);
        }

        public async Task<T> QueryFirst<T>(string query, object param = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<T>(query, param);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string query, object param = null)
        {
            return await Connection.QueryMultipleAsync(query, param, commandTimeout: 1000);
        }
    }
}