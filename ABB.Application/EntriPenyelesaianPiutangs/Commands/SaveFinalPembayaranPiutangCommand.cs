using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class SaveFinalPembayaranPiutangCommand : IRequest<int>
    {
         public string NoBukti { get; set; }
       
    }

    public class SaveFinalPembayaranPiutangCommandHandler : IRequestHandler<SaveFinalPembayaranPiutangCommand, int>
    {
        private readonly IDbContextPstNota _context;
        public SaveFinalPembayaranPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(SaveFinalPembayaranPiutangCommand request, CancellationToken cancellationToken)
        {
            var tempList = await _context.EntriPenyelesaianPiutangTemp
                .Where(x => x.NoBukti == request.NoBukti)
                .ToListAsync(cancellationToken);

            if (!tempList.Any())
                throw new Exception("Tidak ada data di tabel TEMP untuk Piutang ini.");


            var PiutangHeader = await _context.HeaderPenyelesaianUtang
            .FirstOrDefaultAsync(v => v.NomorBukti == request.NoBukti, cancellationToken);

            if (PiutangHeader == null)
                throw new Exception("Piutang Induk tidak ditemukan.");
                
            PiutangHeader.FlagFinal = true;
            _context.HeaderPenyelesaianUtang.Update(PiutangHeader);


            foreach (var temp in tempList)
            {
                _context.EntriPenyelesaianPiutang.Add(new ABB.Domain.Entities.EntriPenyelesaianPiutang
                {
                    NoBukti = temp.NoBukti,
                    No = temp.No,
                    FlagPembayaran = temp.FlagPembayaran,
                    DebetKredit = temp.DebetKredit,
                    KodeAkun = temp.KodeAkun,
                    NoNota = temp.NoNota,
                    KodeMataUang = temp.KodeMataUang,
                    TotalBayarOrg = temp.TotalBayarOrg,
                    TotalBayarRp = temp.TotalBayarRp
                });
            }

            // 1. Simpan ke tabel fisik
            await _context.SaveChangesAsync(cancellationToken);

            // 2. Jalankan SP Update Saldo
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_postpenyelesaianpiutangfinal @p0", 
                new[] { request.NoBukti }, 
                cancellationToken
            );

            return tempList.Count;
        }
    }
}