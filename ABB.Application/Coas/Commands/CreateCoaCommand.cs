using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using CoaEntity = ABB.Domain.Entities.Coa;


namespace ABB.Application.Coas.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateCoaCommand : IRequest<string>
    {
        public string Kode { get; set; }
        public string Nama { get; set; }
        public string Dept { get; set; }
        public string Type { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateCoaCommandHandler : IRequestHandler<CreateCoaCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateCoaCommand request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new CoaEntity
            {
                gl_kode = request.Kode,
                gl_nama = request.Nama,
                gl_dept = request.Dept,
                gl_type = request.Type

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.Coa.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.gl_kode;
        }
    }
}