using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Commands
{
    public class DeleteJurnalMemorial104HeaderCommand : IRequest<bool>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class DeleteJurnalMemorial104HeaderCommandHandler : IRequestHandler<DeleteJurnalMemorial104HeaderCommand, bool>
    {
        private readonly IDbContextPstNota _context;

        public DeleteJurnalMemorial104HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteJurnalMemorial104HeaderCommand request, CancellationToken cancellationToken)
        {
            // 1. Hapus Detail dulu (Cascade manual)
            var details = await _context.DetailJurnalMemorial104
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (details.Any())
            {
                _context.DetailJurnalMemorial104.RemoveRange(details);
            }

            // 2. Hapus Header
            var header = await _context.JurnalMemorial104
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (header != null)
            {
                _context.JurnalMemorial104.Remove(header);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
    }
}