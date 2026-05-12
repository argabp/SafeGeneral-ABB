using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanKeuangan.Queries;
using ABB.Domain.Entities; 
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.ProsesTutupBulan.Commands
{
    public class ProsesTutupBulanCommand : IRequest<Unit>
    {
        public string ThnPrd { get; set; }
        public string BlnPrd { get; set; }
        public string UserUpdate { get; set; }
    }

    public class ProsesTutupBulanCommandHandler : IRequestHandler<ProsesTutupBulanCommand, Unit>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMediator _mediator; 

        public ProsesTutupBulanCommandHandler(IDbContextPstNota context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProsesTutupBulanCommand request, CancellationToken cancellationToken)
        {  
            decimal thn = decimal.Parse(request.ThnPrd);
            short bln = short.Parse(request.BlnPrd);
            
            // ==========================================================
            // 1. JALANKAN SP TERLEBIH DAHULU (Update/Reset Rekap Jurnal)
            // ==========================================================
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_proses_tutup_bulan {0}, {1}",
                thn,     
                bln 
            );

            // ==========================================================
            // 2. TARIK DATA LABA RUGI SETELAH SP SELESAI
            // ==========================================================
            decimal labaRugiAkumulasi = 0;
            try
            {
                var requestLabaRugi = new GetLaporanLabaRugiQuery
                {
                    TipeLaporan = "LABA RUGI (BULAN)", 
                    Bulan = (int)bln,
                    Tahun = (int)thn
                };

                // KODE INI YANG SEMPAT HILANG DI FILE KAMU
                var laporanResponse = await _mediator.Send(requestLabaRugi, cancellationToken);
                labaRugiAkumulasi = laporanResponse.LabaRugiSetelahPajak;
            }
            catch (Exception ex)
            {
                throw new Exception("Gagal menarik Laporan Laba Rugi setelah SP: " + ex.Message);
            }

            // ==========================================================
            // 3. INSERT HASIL AKUMULASI KE TABEL REKAP JURNAL 62
            // ==========================================================
            try
            {
                // Eksekusi insert hanya jika nominalnya tidak 0
                if (Math.Abs(labaRugiAkumulasi) > 0)
                {
                    // [LOGIKA SAKTI D/K]: Jika minus -> Kredit (K), jika plus/nol -> Debet (D)
                    string dkFlag = labaRugiAkumulasi < 0 ? "D" : "K";

                    // Nominal yang masuk ke database HARUS SELALU POSITIF
                    decimal nilaiIdrPositif = Math.Abs(labaRugiAkumulasi);

                    // CONVERT KE DOUBLE KARENA gl_nilai_idr DI DATABASE ADALAH float(53)
                    double nilaiIdrFloat = Convert.ToDouble(nilaiIdrPositif);

                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO abb_rekapjurnal62 (gl_akun, gl_dk, thn, bln, gl_nilai_idr) VALUES ({0}, {1}, {2}, {3}, {4})",
                        "24700000", // <-- Pastikan jumlah digit ini sudah persis dengan yang ada di database ya!
                        dkFlag, 
                        (int)thn, 
                        (int)bln, 
                        nilaiIdrFloat
                    );
                }
            }
            catch (Exception ex)
            {
                // Gunakan throw langsung biar pesan error aslinya meledak kalau ada masalah constraint DB
                throw new Exception("Gagal meng-insert Laba Rugi ke RekapJurnal saat Tutup Bulan: " + ex.Message);
            }
            
            return Unit.Value;
        }
    }
}