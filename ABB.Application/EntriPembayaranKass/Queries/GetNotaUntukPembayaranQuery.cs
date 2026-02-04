using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Data; 
using System.Data.Common; 
using ABB.Application.InquiryNotaProduksis.Queries;

namespace ABB.Application.EntriPembayaranKass.Queries
{
    // 1. QUERY
    public class GetNotaUntukPembayaranQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string JenisAsset { get; set; }
    }

    // 2. HANDLER
    public class GetNotaUntukPembayaranQueryHandler : IRequestHandler<GetNotaUntukPembayaranQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;

        public GetNotaUntukPembayaranQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(GetNotaUntukPembayaranQuery request, CancellationToken cancellationToken)
        {
            var listResult = new List<InquiryNotaProduksiDto>();

            var connection = _context.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync(cancellationToken);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_GetNotaUntukPembayaran";
                    command.CommandType = CommandType.StoredProcedure;

                    // Masukkan Parameter
                    AddParameter(command, "@SearchKeyword", request.SearchKeyword);
                    AddParameter(command, "@JenisAsset", request.JenisAsset);
                    AddParameter(command, "@StartDate", request.StartDate);
                    AddParameter(command, "@EndDate", request.EndDate);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            var dto = new InquiryNotaProduksiDto
                            {
                                // --- Data Utama ---
                                no_nd = GetValue<string>(reader, "no_nd"),
                                no_pl = GetValue<string>(reader, "no_pl"),
                                saldo = GetValue<decimal?>(reader, "saldo"),
                                curensi = GetValue<string>(reader, "curensi"),
                                
                                // Di SP tadi, kita sudah set: p.curensi AS kd_mtu
                                kd_mtu = GetValue<string>(reader, "kd_mtu"), 
                                
                                nm_cust = GetValue<string>(reader, "nm_cust"),
                                nm_cust2 = GetValue<string>(reader, "nm_cust2"),

                                // --- LOGIC ATASAN (AUTO FILL) ---
                                DefaultKodeAkun = GetValue<string>(reader, "DefaultKodeAkun"),
                                DefaultDK = GetValue<string>(reader, "DefaultDK")
                            };

                            listResult.Add(dto);
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return listResult;
        }

        private void AddParameter(DbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        private T GetValue<T>(DbDataReader reader, string columnName)
        {
            // Fungsi aman biar gak error kalau kolom NULL
            try 
            {
                var ordinal = reader.GetOrdinal(columnName);
                if (reader.IsDBNull(ordinal))
                    return default(T);
                return (T)reader.GetValue(ordinal);
            }
            catch
            {
                // Kalau kolom gak ketemu di SP, return default aja biar gak crash
                return default(T);
            }
        }
    }
}