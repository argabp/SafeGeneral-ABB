using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.EntriPeriodes.Commands
{
    public class UpdateEntriPeriodeCommand : IRequest<Unit>
    {
        // Key (untuk mencari data)
        public decimal ThnPrd { get; set; }
        public short BlnPrd { get; set; }

        // Data yang bisa diubah
        public DateTime? TglMul { get; set; }
        public DateTime? TglAkh { get; set; }
        public string FlagClosing { get; set; }
    }

    public class UpdatePeriodeCommandHandler : IRequestHandler<UpdateEntriPeriodeCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdatePeriodeCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateEntriPeriodeCommand request, CancellationToken cancellationToken)
        {
            // Cari berdasarkan Composite Key
            var entity = await _context.EntriPeriode
                .FindAsync(new object[] { request.ThnPrd, request.BlnPrd }, cancellationToken);

            if (entity == null)
            {
                throw new Exception("Periode tidak ditemukan.");
            }

            // Update field
            entity.TglMul = request.TglMul;
            entity.TglAkh = request.TglAkh;
            entity.FlagClosing = request.FlagClosing;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}