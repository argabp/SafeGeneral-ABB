using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EntriPeriodes.Commands
{
    public class DeleteEntriPeriodeCommand : IRequest<Unit>
    {
        public decimal ThnPrd { get; set; }
        public short BlnPrd { get; set; }
    }

    public class DeletePeriodeCommandHandler : IRequestHandler<DeleteEntriPeriodeCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public DeletePeriodeCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEntriPeriodeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.EntriPeriode
                .FindAsync(new object[] { request.ThnPrd, request.BlnPrd }, cancellationToken);

            if (entity == null)
            {
                 throw new Exception("Periode tidak ditemukan.");
            }

            _context.EntriPeriode.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}