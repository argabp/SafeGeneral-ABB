using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class UpdatePenyelesaianPiutangCommand : IRequest
    {
        public int No { get; set; }                 // Id detail pembayaran
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayarOrg { get; set; }
        public decimal? TotalBayarRp { get; set; }
        public string UserBayar { get; set; }
        public string DebetKredit { get; set; }
         public string NoBukti { get; set; }
    }

    public class UpdatePenyelesaianPiutangCommandHandler : IRequestHandler<UpdatePenyelesaianPiutangCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdatePenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {
            // Cari entity berdasarkan No + NoVoucher
            var entity = await _context.EntriPenyelesaianPiutang
                .FirstOrDefaultAsync(e => e.No == request.No && e.NoBukti == request.NoBukti
                , cancellationToken);

            if (entity == null)
            {
                // Kalau tidak ketemu, langsung keluar (atau bisa lempar exception sesuai kebutuhan)
                return Unit.Value;
            }

            // Update field
            entity.FlagPembayaran = request.FlagPembayaran;
            entity.NoNota = request.NoNota;
            entity.KodeAkun = request.KodeAkun;
            entity.KodeMataUang = request.KodeMataUang;
            entity.TotalBayarOrg = request.TotalBayarOrg;
            entity.TotalBayarRp = request.TotalBayarRp;
            entity.DebetKredit = request.DebetKredit;
            entity.UserBayar = request.UserBayar;

            _context.EntriPenyelesaianPiutang.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
