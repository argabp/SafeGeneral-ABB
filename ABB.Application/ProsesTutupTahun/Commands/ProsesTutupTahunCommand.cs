using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.ProsesTutupTahun.Commands
{
    public class ProsesTutupTahunCommand : IRequest<string>
    {
        public int Tahun { get; set; }
        public string UserLogin { get; set; }
    }

    public class ProsesTutupTahunCommandHandler : IRequestHandler<ProsesTutupTahunCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public ProsesTutupTahunCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(ProsesTutupTahunCommand request, CancellationToken cancellationToken)
        {
            // Panggil SP Sakti yang sudah kita buat
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_proses_tutup_tahun {0}, {1}", 
                request.Tahun, 
                request.UserLogin
            );

            return "OK";
        }
    }
}