using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Commands
{
    public class CreatePembayaranBankCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public int? TotalBayar { get; set; }
        public string FlagPembayaran { get; set; }
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
         public string DebetKredit { get; set; }
         public decimal? TotalDlmRupiah { get; set; }
    }

    public class CreatePembayaranBankCommandHandler : IRequestHandler<CreatePembayaranBankCommand, int>
    {
        private readonly IDbContextPstNota _context;
        public CreatePembayaranBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePembayaranBankCommand request, CancellationToken cancellationToken)
        {
            // Cari nomor urut (No) terakhir untuk voucher ini
           var lastNo = await _context.EntriPembayaranBank
            .Where(pb => pb.NoVoucher == request.NoVoucher)
            .OrderByDescending(pb => pb.No)
            .Select(pb => (int?)pb.No)
            .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var entity = new EntriPembayaranBank
            {
                NoVoucher = request.NoVoucher,
                No = lastNo + 1, // Nomor urut baru
                KodeAkun = request.KodeAkun,
                TotalBayar = request.TotalBayar,
               
                FlagPembayaran = request.FlagPembayaran,
                DebetKredit = request.DebetKredit,
                NoNota4 = request.NoNota4,
                KodeMataUang = request.KodeMataUang,
                TotalDlmRupiah = request.TotalDlmRupiah,
                // FlagPosting = "N"
                // Isi field lain yang required atau punya nilai default
            };

            _context.EntriPembayaranBank.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.No;
        }
    }
}