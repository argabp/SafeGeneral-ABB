using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class UpdateFinalPenyelesaianPiutangCommand : IRequest
    {
        public int No { get; set; }                 // Id detail pembayaran
        public string NoBukti { get; set; }       // Nomor voucher induk
        public string FlagPembayaran { get; set; }
        public string NoNota { get; set; }
        public string KodeAkun { get; set; }
        public string KodeMataUang { get; set; }
        public decimal TotalBayarOrg { get; set; }
        public string DebetKredit { get; set; }
        //  public int? Kurs { get; set; }
        public decimal? TotalBayarRp { get; set; }

             public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }

        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
       
    }

    public class UpdateFinalPenyelesaianPiutangCommandHandler : IRequestHandler<UpdateFinalPenyelesaianPiutangCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateFinalPenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateFinalPenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.EntriPenyelesaianPiutang
                .FirstOrDefaultAsync(e => e.No == request.No && e.NoBukti == request.NoBukti, cancellationToken);

            if (entity == null) return Unit.Value;

            // Update field sesuai request
            entity.FlagPembayaran = request.FlagPembayaran;
            entity.NoNota = request.NoNota; // Di sini no nota yang baru diinput user
            entity.KodeAkun = request.KodeAkun;
            entity.KodeMataUang = request.KodeMataUang;
            entity.TotalBayarOrg = request.TotalBayarOrg;
            entity.DebetKredit = request.DebetKredit;
            entity.TotalBayarRp = request.TotalBayarRp;
            entity.TanggalUpdate = DateTime.Now;
            entity.KodeUserUpdate = request.KodeUserUpdate;

            await _context.SaveChangesAsync(cancellationToken);

            // Panggil SP Hitung Ulang untuk Nota ini (Self-Healing)
            // SP ini akan menghitung total semua pembayaran untuk NoNota ini
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_updatefinalpiutang @p0", 
                new[] { request.NoNota }, 
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
