using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using KasBankEntity = ABB.Domain.Entities.KasBank;

namespace ABB.Application.KasBanks.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateKasBankCommand : IRequest<string>
    {
        public string Kode { get; set; }
        public string Keterangan { get; set; }
        public string NoRekening { get; set; }
        public string NoPerkiraan { get; set; }
        public string Kasbank { get; set; }
        public decimal? Saldo { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateKasBankCommandHandler : IRequestHandler<CreateKasBankCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateKasBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateKasBankCommand request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new KasBankEntity
            {
                Kode = request.Kode,
                Keterangan = request.Keterangan,
                NoRekening = request.NoRekening,
                NoPerkiraan = request.NoPerkiraan,
                TipeKasBank = request.Kasbank,
                Saldo = request.Saldo
            };

            // 2. Tambahkan entity baru ke DbContext
            _context.KasBank.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.Kode;
        }
    }
}