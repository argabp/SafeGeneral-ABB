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
                var final = new ABB.Domain.Entities.EntriPenyelesaianPiutang
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
                   
                    // FlagPosting = "N"
                };

                _context.EntriPenyelesaianPiutang.Add(final);
            }

            _context.EntriPenyelesaianPiutangTemp.RemoveRange(tempList);

            // var affectedRows = await _context.SaveChangesAsync(cancellationToken);

           

            // Kembalikan jumlah data detail yang berhasil dipindah
            // return tempList.Count;
            return  await _context.SaveChangesAsync(cancellationToken);;
        }
    }
}