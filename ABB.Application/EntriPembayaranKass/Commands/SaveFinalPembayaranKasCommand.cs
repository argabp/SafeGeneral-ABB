using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    public class SaveFinalPembayaranKasCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
    }

    public class SaveFinalPembayaranKasCommandHandler : IRequestHandler<SaveFinalPembayaranKasCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public SaveFinalPembayaranKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(SaveFinalPembayaranKasCommand request, CancellationToken cancellationToken)
        {
            // 1. Ambil Data dari Tabel TEMP
            var tempList = await _context.EntriPembayaranKasTemp
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (!tempList.Any())
                throw new Exception("Tidak ada data di tabel TEMP untuk voucher ini.");

            // 2. Update Header jadi Final
            var voucherHeader = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");
                
            voucherHeader.FlagFinal = true;
            _context.VoucherKas.Update(voucherHeader);
            
            // 3. Pindahkan Data ke Tabel FINAL (abb_pembayaran_kas)
            foreach (var temp in tempList)
            {
                var final = new ABB.Domain.Entities.EntriPembayaranKas
                {
                    NoVoucher = temp.NoVoucher,
                    No = temp.No,
                    FlagPembayaran = temp.FlagPembayaran,
                    DebetKredit = temp.DebetKredit,
                    KodeAkun = temp.KodeAkun,
                    NoNota4 = temp.NoNota4,
                    KodeMataUang = temp.KodeMataUang,
                    TotalBayar = temp.TotalBayar, // Ini masuk Positif
                    TotalDlmRupiah = temp.TotalDlmRupiah,
                    Kurs = temp.Kurs,
                    FlagPosting = "N"
                };

                _context.EntriPembayaranKas.Add(final);
            }

            // 4. Hapus Data Temp (Opsional, aktifkan jika perlu)
            // _context.EntriPembayaranKasTemp.RemoveRange(tempList);

            // ============================================================
            // LANGKAH KRUSIAL: SIMPAN DULU KE DATABASE
            // ============================================================
            // Kita wajib SaveChangesAsync DULU supaya data masuk ke tabel fisik.
            // Kalau belum disave, SP tidak akan menemukan data vouchernya.
            await _context.SaveChangesAsync(cancellationToken);


            // ============================================================
            // LANGKAH TERAKHIR: PANGGIL STORED PROCEDURE
            // ============================================================
            // SP ini yang akan melakukan update saldo dengan logika matematika benar
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_postpembayarankasfinal @p0", 
                new[] { request.NoVoucher }, 
                cancellationToken
            );

            return tempList.Count;
        }
    }
}
