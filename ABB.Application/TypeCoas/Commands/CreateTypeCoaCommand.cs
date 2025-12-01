using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using TypeCoaEntity = ABB.Domain.Entities.TypeCoa;


namespace ABB.Application.TypeCoas.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateTypeCoaCommand : IRequest<string>
    {
        public string Type { get; set; }
        public string Nama { get; set; }
        public string Pos { get; set; }
        public string Dk { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateTypeCoaCommandHandler : IRequestHandler<CreateTypeCoaCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateTypeCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateTypeCoaCommand request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new TypeCoaEntity
            {
                Type = request.Type,
                Nama = request.Nama,
                Pos = request.Pos,
                Dk = request.Dk

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.TypeCoa.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.Type;
        }
    }
}