using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KasBankEntity = ABB.Domain.Entities.KasBank;

namespace ABB.Application.KasBanks.Commands
{
    public class DeleteKasBankCommand : IRequest
    {
        public string Kode { get; set; }
    }

    public class DeleteKasBankCommandHandler : IRequestHandler<DeleteKasBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteKasBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteKasBankCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.KasBank
                .FirstOrDefaultAsync(kb => kb.Kode == request.Kode, cancellationToken);

            if (entity != null)
            {
                _context.KasBank.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}