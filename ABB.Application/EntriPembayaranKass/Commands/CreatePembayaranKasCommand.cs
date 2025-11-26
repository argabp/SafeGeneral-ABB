using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    public class CreatePembayaranKasCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public int? TotalBayar { get; set; }
        public string FlagPembayaran { get; set; }
        public string DebetKredit { get; set; }
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
         public int? Kurs { get; set; }
        public decimal? TotalDlmRupiah { get; set; }
    }

    public class CreatePembayaranKasCommandHandler : IRequestHandler<CreatePembayaranKasCommand, int>
    {
        private readonly IDbContextPstNota _context;
        public CreatePembayaranKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePembayaranKasCommand request, CancellationToken cancellationToken)
        {
            // Cari nomor urut (No) terakhir untuk voucher ini
            var lastNo = await _context.EntriPembayaranKasTemp
            .Where(pb => pb.NoVoucher == request.NoVoucher)
            .OrderByDescending(pb => pb.No)
            .Select(pb => (int?)pb.No)
            .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var entity = new EntriPembayaranKasTemp
            {
                NoVoucher = request.NoVoucher,
                No = lastNo + 1, // Nomor urut baru
                KodeAkun = request.KodeAkun,
                TotalBayar = request.TotalBayar,
                FlagPembayaran = request.FlagPembayaran,
                NoNota4 = request.NoNota4,
                DebetKredit = request.DebetKredit,
                KodeMataUang = request.KodeMataUang,
                TotalDlmRupiah = request.TotalDlmRupiah,
                Kurs = request.Kurs,
                FlagPosting = "N"
            };

            _context.EntriPembayaranKasTemp.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.No;
        }
    }
}