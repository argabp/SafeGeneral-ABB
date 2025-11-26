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
    public class CreatePenyelesaianPiutangCommand : IRequest<int>
    {
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

    public class CreatePenyelesaianPiutangCommandHandler : IRequestHandler<CreatePenyelesaianPiutangCommand, int>
    {
        private readonly IDbContextPstNota _context;
        public CreatePenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {
            // Cari nomor urut (No) terakhir untuk voucher ini
            var lastNo = await _context.EntriPenyelesaianPiutangTemp
                .Where(pb => pb.NoBukti == request.NoBukti)
                .OrderByDescending(pb => pb.No)
                .Select(pb => (int?)pb.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var entity = new EntriPenyelesaianPiutangTemp
            {
                NoBukti = request.NoBukti,
                No = lastNo + 1, // Nomor urut baru
                KodeAkun = request.KodeAkun,
                TotalBayarOrg = request.TotalBayarOrg,
                TotalBayarRp = request.TotalBayarRp,
               
                FlagPembayaran = request.FlagPembayaran,
                DebetKredit = request.DebetKredit,
                NoNota = request.NoNota,
                KodeMataUang = request.KodeMataUang,
                
                // Isi field lain yang required atau punya nilai default
            };

            _context.EntriPenyelesaianPiutangTemp.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.No;
        }
    }
}