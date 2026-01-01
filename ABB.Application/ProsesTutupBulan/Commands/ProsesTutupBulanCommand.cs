using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.ProsesTutupBulan.Commands
{
    public class ProsesTutupBulanCommand : IRequest<Unit>
    {
        public string ThnPrd { get; set; }
        public string BlnPrd { get; set; }
        public string UserUpdate { get; set; }
    }

    public class ProsesTutupBulanCommandHandler : IRequestHandler<ProsesTutupBulanCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public ProsesTutupBulanCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ProsesTutupBulanCommand request, CancellationToken cancellationToken)
        {  

            decimal thn = decimal.Parse(request.ThnPrd);
            short bln = short.Parse(request.BlnPrd);
            
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_proses_tutup_bulan {0}, {1}",
                thn,     
                bln 
            );
            
            return Unit.Value;


            // decimal thn = decimal.Parse(request.ThnPrd);
            // short bln = short.Parse(request.BlnPrd);
            // var entity = await _context.EntriPeriode
            //     .FirstOrDefaultAsync(x => x.ThnPrd == thn && x.BlnPrd == bln, cancellationToken);

            // if (entity == null) 
            // {
            //     throw new Exception($"Periode {request.ThnPrd}-{request.BlnPrd} tidak ditemukan di tabel Periode.");
            // }
            // entity.FlagClosing = "Y"; 
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}