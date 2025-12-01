using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.TypeCoas.Commands
{
    public class UpdateTypeCoaCommand : IRequest<Unit>
    {
        public string Type { get; set; } // Primary Key (biasanya tidak diubah)
        public string Nama { get; set; }
        public string Pos { get; set; }
        public string Dk { get; set; }
    }

    public class UpdateTypeCoaCommandHandler : IRequestHandler<UpdateTypeCoaCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateTypeCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTypeCoaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TypeCoa.FindAsync(new object[] { request.Type }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TypeCoa), request.Type);
            }

            // Update field
            entity.Nama = request.Nama;
            entity.Pos = request.Pos;
            entity.Dk = request.Dk;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}