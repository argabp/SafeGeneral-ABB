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
            var tempList = await _context.EntriPembayaranKasTemp
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (!tempList.Any())
                throw new Exception("Tidak ada data di tabel TEMP untuk voucher ini.");

            var voucherHeader = await _context.VoucherKas
            .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");
                
            voucherHeader.FlagFinal = true;
            _context.VoucherKas.Update(voucherHeader);
            
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
                    TotalBayar = temp.TotalBayar,
                    TotalDlmRupiah = temp.TotalDlmRupiah,
                    Kurs = temp.Kurs,
                    FlagPosting = "N"
                };

                _context.EntriPembayaranKas.Add(final);
            }

            //_context.EntriPembayaranKasTemp.RemoveRange(tempList);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);

            return tempList.Count;
        }
    }
}
