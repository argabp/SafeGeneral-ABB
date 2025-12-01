using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Coa117Entity = ABB.Domain.Entities.Coa117;


namespace ABB.Application.Coas117.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateCoa117Command : IRequest<string>
    {
        public string Kode { get; set; }
        public string Nama { get; set; }
        public string Dept { get; set; }
        public string Type { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateCoa117CommandHandler : IRequestHandler<CreateCoa117Command, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateCoa117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateCoa117Command request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new Coa117Entity
            {
                gl_kode = request.Kode,
                gl_nama = request.Nama,
                gl_dept = request.Dept,
                gl_type = request.Type

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.Coa117.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.gl_kode;
        }
    }
}