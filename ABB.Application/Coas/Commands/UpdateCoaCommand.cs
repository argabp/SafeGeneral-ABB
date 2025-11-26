using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas.Commands
{
    // Ini adalah "Surat Perintah" untuk memperbarui data
    public class UpdateCoaCommand : IRequest
    {
        public string Kode { get; set; }
        public string Nama { get; set; }
        public string Dept { get; set; }
        public string Type { get; set; }
    }

    // Ini adalah "Petugas Pelaksana" untuk perintah di atas
    public class UpdateCoaCommandHandler : IRequestHandler<UpdateCoaCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCoaCommand request, CancellationToken cancellationToken)
        {
            // 1. Cari data yang ada di database berdasarkan Primary Key (NoVoucher)
            var entity = await _context.Coa
                .FirstOrDefaultAsync(v => v.gl_kode == request.Kode, cancellationToken);

            // 2. Jika data ditemukan, update propertinya
            if (entity != null)
            {
                entity.gl_kode = request.Kode;
                entity.gl_nama = request.Nama;
                entity.gl_dept = request.Dept;
                entity.gl_type = request.Type;
                // 3. Simpan perubahan ke database
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value; // Mengindikasikan proses selesai
        }
    }
}