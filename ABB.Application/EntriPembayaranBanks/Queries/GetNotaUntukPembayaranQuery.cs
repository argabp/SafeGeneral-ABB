using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Data; // Wajib untuk ADO.NET
using System.Data.Common;
using ABB.Application.InquiryNotaProduksis.Queries; // Kita masih butuh DTO-nya

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    // 1. QUERY BARU
    public class GetNotaUntukPembayaranQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string JenisAsset { get; set; }
    }

    // 2. HANDLER BARU
    public class GetNotaUntukPembayaranQueryHandler : IRequestHandler<GetNotaUntukPembayaranQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetNotaUntukPembayaranQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(GetNotaUntukPembayaranQuery request, CancellationToken cancellationToken)
        {
            var listResult = new List<InquiryNotaProduksiDto>();

            // 1. Ambil Koneksi dari EF Core
            var connection = _context.Database.GetDbConnection();

            try
            {
                // Pastikan koneksi terbuka
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync(cancellationToken);

                // 2. Buat Command
                using (var command = connection.CreateCommand())
                {
                    // Panggil SP yang SAMA dengan modul KAS
                    command.CommandText = "sp_GetNotaUntukPembayaran";
                    command.CommandType = CommandType.StoredProcedure;

                    // 3. Masukkan Parameter (Handle Null value)
                    AddParameter(command, "@SearchKeyword", request.SearchKeyword);
                    AddParameter(command, "@JenisAsset", request.JenisAsset);
                    AddParameter(command, "@StartDate", request.StartDate);
                    AddParameter(command, "@EndDate", request.EndDate);

                    // 4. Eksekusi Reader
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            // 5. MAPPING MANUAL (Satu per satu)
                            // Hati-hati: Nama string di reader["..."] harus SAMA PERSIS dengan kolom output SP
                            var dto = new InquiryNotaProduksiDto
                            {
                                // --- Data Utama ---
                                no_nd = GetValue<string>(reader, "no_nd"),
                                no_pl = GetValue<string>(reader, "no_pl"),
                                saldo = GetValue<decimal?>(reader, "saldo"),
                                curensi = GetValue<string>(reader, "curensi"),
                                
                                // Di SP tadi, kita sudah set: ISNULL(m.kd_mtu, '') AS kd_mtu
                                kd_mtu = GetValue<string>(reader, "kd_mtu"), 
                                
                                nm_cust2 = GetValue<string>(reader, "nm_cust2"),
                                nm_cust = GetValue<string>(reader, "nm_cust"),

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
                // Tutup koneksi jika dibuka manual
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return listResult;
        }

        // --- Helper Function Biar Gak Ribet Nulis DBNull ---
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