using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.PostingVoucherBank.Commands
{
    public class PostingVoucherBankCommand : IRequest
    {
        public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

    public class PostingVoucherBankCommandHandler : IRequestHandler<PostingVoucherBankCommand>
    {
        private readonly IDbContextPstNota _context;
        
        public PostingVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingVoucherBankCommand request, CancellationToken cancellationToken)
        {
            // Ambil User ID yang sedang login
            var userId = request.UserId ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            // Loop setiap NoVoucher yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                // =========================================================
                // 1. VALIDASI KODE AKUN KE TABEL MAPPING
                // =========================================================
                
                // Ambil semua akun dari Header (Pastikan nama DbSet dan Property sesuai Entity Anda)
                var akunHeader = await _context.VoucherBank 
                    .Where(v => v.NoVoucher == noVoucher) 
                    .Select(v => v.KodeAkun)
                    .ToListAsync(cancellationToken);

                // Ambil semua akun dari Detail (Pastikan nama DbSet dan Property sesuai Entity Anda)
                var akunDetail = await _context.EntriPembayaranBank 
                    .Where(d => d.NoVoucher == noVoucher)
                    .Select(d => d.KodeAkun)
                    .ToListAsync(cancellationToken);

                // Gabungkan akun header dan detail, hilangkan yang kosong, dan buat unik (tidak duplikat)
                var semuaAkunUnik = akunHeader.Concat(akunDetail)
                                              .Where(a => !string.IsNullOrWhiteSpace(a))
                                              .Distinct()
                                              .ToList();

                // Cek satu per satu apakah akun tersebut ada di EntriMapping
                foreach (var akun in semuaAkunUnik)
                {
                    var isMapped = await _context.EntriMapping
                        .AnyAsync(m => m.gl_akun104 == akun, cancellationToken);

                    if (!isMapped)
                    {
                        // Lempar error! Controller akan menangkap ini dan mengirim ke JS Anda
                        throw new Exception($"Gagal Posting! Kode akun '{akun}' pada Voucher '{noVoucher}' belum di-mapping.");
                    }
                }
                // =========================================================

                // 2. JIKA VALIDASI AMAN, LANJUT EKSEKUSI SP
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_posting_voucher_bank {0}, {1}, {2}",
                    noVoucher,      // Masuk ke {0} -> @NoVoucher
                    userId,         // Masuk ke {1} -> @KodeUserUpdate
                    tanggalPosting  // Masuk ke {2} -> @TanggalPosting
                );
            }

            return Unit.Value;
        }
    }
}