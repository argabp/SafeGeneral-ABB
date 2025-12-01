using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using TipeAkun117Entity = ABB.Domain.Entities.TipeAkun117;


namespace ABB.Application.TipeAkuns117.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateTipeAkun117Command : IRequest<string>
    {
        public string Kode { get; set; }
        public string NamaTipe { get; set; }
        public string Pos { get; set; }
        public string DebetKredit { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateTipeAkun117CommandHandler : IRequestHandler<CreateTipeAkun117Command, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateTipeAkun117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateTipeAkun117Command request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new TipeAkun117Entity
            {
                Kode = request.Kode,
                NamaTipe = request.NamaTipe,
                Pos = request.Pos,
                DebetKredit = request.DebetKredit

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.TipeAkun117.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.Kode;
        }
    }
}