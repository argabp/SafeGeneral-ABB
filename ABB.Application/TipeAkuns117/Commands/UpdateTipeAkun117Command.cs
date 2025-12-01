using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.TipeAkuns117.Commands
{
    public class UpdateTipeAkun117Command : IRequest<Unit>
    {
        public string Kode { get; set; } // Primary Key (biasanya tidak diubah)
        public string NamaTipe { get; set; }
        public string Pos { get; set; }
        public string DebetKredit { get; set; }
    }

    public class UpdateTipeAkun117CommandHandler : IRequestHandler<UpdateTipeAkun117Command, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateTipeAkun117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTipeAkun117Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.TipeAkun117.FindAsync(new object[] { request.Kode }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TipeAkun117), request.Kode);
            }

            // Update field
            entity.NamaTipe = request.NamaTipe;
            entity.Pos = request.Pos;
            entity.DebetKredit = request.DebetKredit;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}