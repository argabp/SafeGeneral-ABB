using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    public class UpdatePembayaranKasCommand : IRequest
    {
        public int No { get; set; }                 // Id detail pembayaran
        public string NoVoucher { get; set; }       // Nomor voucher induk
        public string FlagPembayaran { get; set; }
        public string NoNota4 { get; set; }
        public string KodeAkun { get; set; }
        public string KodeMataUang { get; set; }
        public int TotalBayar { get; set; }
        public string DebetKredit { get; set; }
        public decimal? TotalDlmRupiah { get; set; }
    }

    public class UpdatePembayaranKasCommandHandler : IRequestHandler<UpdatePembayaranKasCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdatePembayaranKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePembayaranKasCommand request, CancellationToken cancellationToken)
        {
            // Cari entity berdasarkan No + NoVoucher
            var entity = await _context.EntriPembayaranKas
                .FirstOrDefaultAsync(e => e.No == request.No && e.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null)
            {
                // Kalau tidak ketemu, langsung keluar (atau bisa lempar exception sesuai kebutuhan)
                return Unit.Value;
            }

            // Update field
            entity.FlagPembayaran = request.FlagPembayaran;
            entity.NoNota4 = request.NoNota4;
            entity.KodeAkun = request.KodeAkun;
            entity.KodeMataUang = request.KodeMataUang;
            entity.TotalBayar = request.TotalBayar;
            entity.DebetKredit = request.DebetKredit;
            entity.TotalDlmRupiah = request.TotalDlmRupiah;

            _context.EntriPembayaranKas.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
