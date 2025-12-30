using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.CancelTutupBulan.Commands
{
    public class CancelTutupBulanCommand : IRequest<Unit>
    {
        public string ThnPrd { get; set; }
        public string BlnPrd { get; set; }
        public string UserUpdate { get; set; }
    }

    public class CancelTutupBulanCommandHandler : IRequestHandler<CancelTutupBulanCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public CancelTutupBulanCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelTutupBulanCommand request, CancellationToken cancellationToken)
            {
               // 1. Parsing Tipe Data (Sesuai Entity EntriPeriode)
            decimal thn = decimal.Parse(request.ThnPrd);
            short bln = short.Parse(request.BlnPrd);

            // 2. Cari di Tabel PERIODE
            var entity = await _context.EntriPeriode
                .FirstOrDefaultAsync(x => x.ThnPrd == thn && x.BlnPrd == bln, cancellationToken);

            if (entity == null) 
            {
                throw new Exception($"Periode {request.ThnPrd}-{request.BlnPrd} tidak ditemukan di tabel Periode.");
            }

            // 3. Update Flag
            entity.FlagClosing = "N"; 
            
            // Update audit trail jika ada di entity EntriPeriode
            // entity.UserUpdate = request.UserUpdate;

            // 4. Simpan (Sekarang pasti bisa karena ini Tabel Fisik, bukan View)
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
            }
    }
}