using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.Entities;
using System;

namespace ABB.Application.PostingPenyelesaianPiutang.Commands
{
    public class PostingPenyelesaianPiutangCommand : IRequest
    {
         public List<string> Data { get; set; } // Berisi daftar NomorBukti
        public string UserId { get; set; }
    }

    public class PostingPenyelesaianPiutangCommandHandler : IRequestHandler<PostingPenyelesaianPiutangCommand>
    {
        private readonly IDbContextPstNota _context;

        public PostingPenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingPenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {
            // Ambil User ID yang sedang login
            var userId = request.UserId ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            // Loop setiap NomorBukti yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                // =========================================================
                // 1. VALIDASI KODE AKUN KE TABEL MAPPING
                // =========================================================
                
                // // Ambil semua akun dari Header Penyelesaian Utang/Piutang
                // var akunHeader = await _context.HeaderPenyelesaianUtang 
                //     .Where(h => h.NomorBukti == noVoucher) 
                //     .Select(h => h.KodeAkun)
                //     .ToListAsync(cancellationToken);

                // // Ambil semua akun dari Detail Penyelesaian Utang/Piutang
                // // (Sesuaikan nama Entity "PenyelesaianUtang" jika di project Anda berbeda)
                // var akunDetail = await _context.EntriPenyelesaianPiutang 
                //     .Where(d => d.NoBukti == noVoucher)
                //     .Select(d => d.KodeAkun)
                //     .ToListAsync(cancellationToken);

                // // Gabungkan akun header dan detail, hilangkan yang kosong, dan buat unik
                // var semuaAkunUnik = akunHeader.Concat(akunDetail)
                //                               .Where(a => !string.IsNullOrWhiteSpace(a))
                //                               .Distinct()
                //                               .ToList();

                // // Cek satu per satu apakah akun tersebut ada di EntriMapping
                // foreach (var akun in semuaAkunUnik)
                // {
                //     var isMapped = await _context.EntriMapping
                //         .AnyAsync(m => m.gl_akun104 == akun, cancellationToken);

                //     if (!isMapped)
                //     {
                //         // Lempar error jika belum di-mapping
                //         throw new Exception($"Gagal Posting! Kode akun '{akun}' pada Nomor Bukti '{noVoucher}' belum di-mapping.");
                //     }
                // }
                // =========================================================

                // 2. JIKA VALIDASI AMAN, LANJUT EKSEKUSI SP
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_posting_penyelesaian_piutang {0}, {1}, {2}",
                    noVoucher,      // Masuk ke {0} -> @NomorBukti
                    userId,         // Masuk ke {1} -> @KodeUserUpdate
                    tanggalPosting  // Masuk ke {2} -> @TanggalPosting
                );
            }

            return Unit.Value;
        }
    }
}