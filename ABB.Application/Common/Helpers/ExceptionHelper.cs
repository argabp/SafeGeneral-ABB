using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Helpers
{
    public static class ExceptionHelper
    {
        public static async Task ExecuteWithLoggingAsync(Func<Task> action, ILogger logger)
        {
            try
            {
                await action();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    foreach (SqlError err in sqlEx.Errors)
                    {
                        logger.LogError($"SQL ERROR {err.Number}: {err.Message}");
                    }
                }
                throw ex.InnerException ?? ex;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex.InnerException ?? ex;
            }
        }

        public static async Task<T> ExecuteWithLoggingAsync<T>(Func<Task<T>> action, ILogger logger)
        {
            try
            {
                return await action();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    foreach (SqlError err in sqlEx.Errors)
                    {
                        logger.LogError($"SQL ERROR {err.Number}: {err.Message}");
                    }
                }
                throw ex.InnerException ?? ex;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex.InnerException ?? ex;
            }
        }
    }
}