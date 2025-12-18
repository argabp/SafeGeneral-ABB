using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class DeleteJurnalMemorial117HeaderCommand : IRequest<bool>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class DeleteJurnalMemorial117HeaderCommandHandler : IRequestHandler<DeleteJurnalMemorial117HeaderCommand, bool>
    {
        private readonly IDbContextPstNota _context;

        public DeleteJurnalMemorial117HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteJurnalMemorial117HeaderCommand request, CancellationToken cancellationToken)
        {
            // 1. Hapus Detail dulu (Cascade manual)
            var details = await _context.JurnalMemorial117Detail
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (details.Any())
            {
                _context.JurnalMemorial117Detail.RemoveRange(details);
            }

            // 2. Hapus Header
            var header = await _context.JurnalMemorial117
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (header != null)
            {
                _context.JurnalMemorial117.Remove(header);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
    }
}