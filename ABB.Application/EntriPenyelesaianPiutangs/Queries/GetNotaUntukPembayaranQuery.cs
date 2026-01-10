using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Data; // Wajib
using System.Data.Common; // Wajib
using ABB.Application.InquiryNotaProduksis.Queries; // Kita masih butuh DTO-nya

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    // 1. QUERY BARU
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

            // 1. Ambil Koneksi
            var connection = _context.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync(cancellationToken);

                // 2. Buat Command SP
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_GetNotaUntukPembayaran"; // SP SAMA DENGAN BANK/KAS
                    command.CommandType = CommandType.StoredProcedure;

                    // 3. Parameter
                    AddParameter(command, "@SearchKeyword", request.SearchKeyword);
                    AddParameter(command, "@JenisAsset", request.JenisAsset);
                    AddParameter(command, "@StartDate", request.StartDate);
                    AddParameter(command, "@EndDate", request.EndDate);

                    // 4. Eksekusi
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
                                
                                // Ambil logic mata uang dari SP (bukan join EF)
                                kd_mtu = GetValue<string>(reader, "kd_mtu"), 
                                
                                nm_cust2 = GetValue<string>(reader, "nm_cust2"),

                                // --- LOGIC AUTO FILL (PENTING) ---
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
            try 
            {
                var ordinal = reader.GetOrdinal(columnName);
                if (reader.IsDBNull(ordinal)) return default(T);
                return (T)reader.GetValue(ordinal);
            }
            catch { return default(T); }
        }
    }
}