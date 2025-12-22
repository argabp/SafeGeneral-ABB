using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class UpdatePenyelesaianPiutangCommand : IRequest, IMapFrom<CreatePenyelesaianPiutangCommand>
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

             public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }

        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }

          public void Mapping(Profile profile)
            {
                // Aturan untuk mengubah CreateCommand menjadi UpdateCommand
                profile.CreateMap<CreatePenyelesaianPiutangCommand, UpdatePenyelesaianPiutangCommand>();
            }
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
            var entity = await _context.EntriPenyelesaianPiutangTemp
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

             entity.TanggalUpdate = DateTime.Now;
            entity.KodeUserUpdate = request.KodeUserUpdate;

            _context.EntriPenyelesaianPiutangTemp.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
