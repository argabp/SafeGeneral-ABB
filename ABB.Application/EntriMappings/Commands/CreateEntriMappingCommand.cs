using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using EntriMappingEntity = ABB.Domain.Entities.EntriMapping;


namespace ABB.Application.EntriMappings.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateEntriMappingCommand : IRequest<string>
    {
        public string gl_akun104 { get; set; }
        public string gl_akun117 { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateEntriMappingCommandHandler : IRequestHandler<CreateEntriMappingCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateEntriMappingCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateEntriMappingCommand request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new EntriMappingEntity
            {
                gl_akun104 = request.gl_akun104,
                gl_akun117 = request.gl_akun117

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.EntriMapping.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.gl_akun104;
        }
    }
}