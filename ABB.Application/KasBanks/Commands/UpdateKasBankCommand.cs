using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.KasBanks.Commands
{
    // =========================
    // COMMAND
    // =========================
    public class UpdateKasBankCommand : IRequest
    {
        // 🔑 COMPOSITE KEY
        public string KodeCabang { get; set; }
        public string TipeKasBank { get; set; }
        public string Kode { get; set; }

        // DATA UPDATE
        public string Keterangan { get; set; }
        public string NoRekening { get; set; }
        public string NoPerkiraan { get; set; }
        public decimal? Saldo { get; set; }
    }

    // =========================
    // HANDLER
    // =========================
    public class UpdateKasBankCommandHandler 
        : IRequestHandler<UpdateKasBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateKasBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(
            UpdateKasBankCommand request, 
            CancellationToken cancellationToken)
        {
            // VALIDASI DASAR
            if (string.IsNullOrEmpty(request.KodeCabang) ||
                string.IsNullOrEmpty(request.TipeKasBank) ||
                string.IsNullOrEmpty(request.Kode))
            {
                return Unit.Value;
            }

            // 🔎 CARI DATA BERDASARKAN COMPOSITE KEY
            var entity = await _context.KasBank
                .FirstOrDefaultAsync(x =>
                    x.KodeCabang == request.KodeCabang &&
                    x.TipeKasBank == request.TipeKasBank &&
                    x.Kode == request.Kode,
                    cancellationToken);

            if (entity == null)
                return Unit.Value;

            // ✏ UPDATE FIELD
            entity.Keterangan = request.Keterangan;
            entity.NoRekening = request.NoRekening;
            entity.NoPerkiraan = request.NoPerkiraan;
            entity.Saldo = request.Saldo;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
