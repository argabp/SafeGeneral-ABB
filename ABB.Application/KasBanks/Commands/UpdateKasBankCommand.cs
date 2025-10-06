using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KasBankEntity = ABB.Domain.Entities.KasBank;

namespace ABB.Application.KasBanks.Commands
{
    public class UpdateKasBankCommand : IRequest
    {
        // Properti yang dibutuhkan untuk update
        public string Kode { get; set; }
        public string Keterangan { get; set; }
        public string NoRekening { get; set; }
        public string NoPerkiraan { get; set; }
        public string TipeKasBank { get; set; }
    }

    public class UpdateKasBankCommandHandler : IRequestHandler<UpdateKasBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateKasBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateKasBankCommand request, CancellationToken cancellationToken)
        {
            // 1. Cari data yang ada di database berdasarkan Kode
            var entity = await _context.KasBank
                .FirstOrDefaultAsync(kb => kb.Kode == request.Kode, cancellationToken);

            if (entity != null)
            {
                // 2. Update propertinya dengan data baru dari request
                entity.Keterangan = request.Keterangan;
                entity.NoRekening = request.NoRekening;
                entity.NoPerkiraan = request.NoPerkiraan;
                entity.TipeKasBank = request.TipeKasBank;

                // 3. Simpan perubahan
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value; // Mengindikasikan proses selesai tanpa mengembalikan data
        }
    }
}