using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
// Pastikan using ini ada untuk memanggil entitas Anda
using EntriMappingEntity = ABB.Domain.Entities.EntriMapping; 

namespace ABB.Application.EntriMappings.Commands
{
    public class UpdateEntriMappingCommand : IRequest
    {
        public string gl_akun104 { get; set; } 
        public string gl_akun117 { get; set; } 
    }

    public class UpdateEntriMappingCommandHandler : IRequestHandler<UpdateEntriMappingCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateEntriMappingCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateEntriMappingCommand request, CancellationToken cancellationToken)
        {
            // 1. CARI DATA LAMA berdasarkan gl_akun104
            var dataLama = await _context.EntriMapping
                .FirstOrDefaultAsync(v => v.gl_akun104 == request.gl_akun104, cancellationToken);

            if (dataLama != null)
            {
                // 2. HAPUS DATA LAMA dari tracking
                _context.EntriMapping.Remove(dataLama);

                // 3. BUAT DATA BARU dengan nilai gl_akun117 yang di-update
                var dataBaru = new EntriMappingEntity
                {
                    gl_akun104 = dataLama.gl_akun104, // Pertahankan 104 yang lama
                    gl_akun117 = request.gl_akun117   // Gunakan 117 yang baru dari inputan
                };

                // 4. MASUKKAN DATA BARU
                _context.EntriMapping.Add(dataBaru);

                // 5. SIMPAN KE DATABASE (EF Core akan otomatis melakukan DELETE lalu INSERT)
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Data lama tidak ditemukan, tidak bisa melakukan proses edit.");
            }

            return Unit.Value;
        }
    }
}