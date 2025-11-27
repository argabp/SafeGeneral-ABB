using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.EntriPeriodes.Commands
{
    public class CreateEntriPeriodeCommand : IRequest<Unit>
    {
        public decimal ThnPrd { get; set; }
        public short BlnPrd { get; set; }
        public DateTime? TglMul { get; set; }
        public DateTime? TglAkh { get; set; }
        public string FlagClosing { get; set; }
    }

    public class CreatePeriodeCommandHandler : IRequestHandler<CreateEntriPeriodeCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public CreatePeriodeCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateEntriPeriodeCommand request, CancellationToken cancellationToken)
        {
            // Cek duplikasi (Composite Key)
            var existing = await _context.EntriPeriode.FindAsync(new object[] { request.ThnPrd, request.BlnPrd }, cancellationToken);
            if (existing != null)
            {
                throw new Exception($"Periode {request.BlnPrd}/{request.ThnPrd} sudah ada.");
            }

            var entity = new EntriPeriode
            {
                ThnPrd = request.ThnPrd,
                BlnPrd = request.BlnPrd,
                TglMul = request.TglMul,
                TglAkh = request.TglAkh,
                FlagClosing = string.IsNullOrEmpty(request.FlagClosing) ? "N" : request.FlagClosing
            };

            _context.EntriPeriode.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}