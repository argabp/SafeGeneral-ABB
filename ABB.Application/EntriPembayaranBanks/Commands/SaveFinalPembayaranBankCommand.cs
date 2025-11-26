using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Commands
{
    public class SaveFinalPembayaranBankCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
    }

    public class SaveFinalPembayaranBankCommandHandler : IRequestHandler<SaveFinalPembayaranBankCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public SaveFinalPembayaranBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(SaveFinalPembayaranBankCommand request, CancellationToken cancellationToken)
        {
            var tempList = await _context.EntriPembayaranBankTemp
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (!tempList.Any())
                throw new Exception("Tidak ada data di tabel TEMP untuk voucher ini.");

            var voucherHeader = await _context.VoucherBank
            .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");
                
            voucherHeader.FlagFinal = true;
            _context.VoucherBank.Update(voucherHeader);

            foreach (var temp in tempList)
            {
                var final = new ABB.Domain.Entities.EntriPembayaranBank
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
                   
                    // FlagPosting = "N"
                };

                _context.EntriPembayaranBank.Add(final);
            }

            _context.EntriPembayaranBankTemp.RemoveRange(tempList);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);

            // Kembalikan jumlah data detail yang berhasil dipindah
            return tempList.Count;
        }
    }
}
