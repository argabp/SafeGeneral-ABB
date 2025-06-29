using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABB.Application.Common.Interfaces
{
    public interface IDbConnectionCSM
    {
        Task ExecuteNonQueryProc(string query, object param = null);
        Task<object> ExecuteScalarProc(string query, object param = null);
        Task<IEnumerable<dynamic>> QueryProc(string query, object param = null);
        Task<IEnumerable<T>> QueryProc<T>(string query, object param = null);
        Task<dynamic> QueryFirstProc(string query, object param = null);
        Task<T> QueryFirstProc<T>(string query, object param = null);

        Task ExecuteNonQuery(string query, object param = null);
        Task<object> ExecuteScalar(string query, object param = null);
        Task<IEnumerable<dynamic>> Query(string query, object param = null);
        Task<IEnumerable<T>> Query<T>(string query, object param = null);
        Task<dynamic> QueryFirst(string query, object param = null);
        Task<T> QueryFirst<T>(string query, object param = null);
    }
}