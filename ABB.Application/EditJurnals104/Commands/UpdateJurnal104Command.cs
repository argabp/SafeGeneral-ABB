using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Application.Common.Interfaces;
using ABB.Application.EditJurnals104.Queries;

namespace ABB.Application.EditJurnals104.Commands
{
    public class UpdateJurnal104Command : IRequest<bool>
    {
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        
        // Data Header
        public string NoBukti { get; set; }
        public DateTime TglBukti { get; set; }
        public string KodeLokasi { get; set; }
        public string GlTran { get; set; } 
        public string KeteranganUtama { get; set; }

        // Data Detail (Grid)
        public List<DetailJurnal104Dto> Details { get; set; }
    }

    public class UpdateJurnal104CommandHandler : IRequestHandler<UpdateJurnal104Command, bool>
    {
        private readonly IDbContextPstNota _context;

        public UpdateJurnal104CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateJurnal104Command request, CancellationToken cancellationToken)
        {
            // 1. Validasi Balance Jurnal (Langsung dihitung di C#)
            decimal totalDebet = request.Details.Where(x => x.DK == "D").Sum(x => x.NilaiIdr);
            decimal totalKredit = request.Details.Where(x => x.DK == "K").Sum(x => x.NilaiIdr);

            if (totalDebet != totalKredit)
            {
                throw new Exception("Total Debet dan Kredit tidak balance (seimbang)! Data gagal disimpan.");
            }

            // 2. Buka Transaksi Database (Supaya aman kalau di tengah jalan ada error)
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // 3. Hapus jurnal lama terlebih dahulu pakai Raw SQL biasa
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM abb_jurnal62 WHERE gl_bukti = {0}", 
                        request.NoBukti
                    );

                    // 4. Looping untuk insert baris baru satu per satu
                    foreach (var item in request.Details)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "EXEC sp_UpdateEditJurnal104 @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11",
                            request.KodeLokasi,
                            request.NoBukti,
                            request.TglBukti,
                            item.NoNota ?? "",
                            item.MataUang ?? "IDR",
                            request.KeteranganUtama,
                            item.NoUrut,
                            item.DK,
                            item.NilaiOrg,
                            item.NilaiIdr,
                            item.KodeAkun,
                            request.UserId
                        );
                    }

                    // 5. Jika sukses semua, Simpan Permanen!
                    await transaction.CommitAsync(cancellationToken);
                    return true;
                }
                catch (Exception)
                {
                    // Jika ada 1 baris yang gagal, batalkan (rollback) semuanya
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}