using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;
// [TAMBAHKAN INI] Agar 'TemplateLapKeu' dikenali
using ABB.Domain.Entities; 

namespace ABB.Application.TemplateLapKeus.Commands
{
    public class UpdateTemplateLapKeuCommand : IRequest
    {
        public long Id { get; set; }
        public string TipeLaporan { get; set; }
        public string TipeBaris { get; set; }
        public string Deskripsi { get; set; }
        public string Rumus { get; set; }
        public string Level { get; set; }
        public int Urutan { get; set; }
    }

    public class UpdateTemplateLapKeuCommandHandler : IRequestHandler<UpdateTemplateLapKeuCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateTemplateLapKeuCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTemplateLapKeuCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TemplateLapKeu.FindAsync(request.Id);

            if (entity == null)
            {
                // Disini errornya tadi, sekarang sudah aman karena ada using ABB.Domain.Entities
                throw new NotFoundException(nameof(TemplateLapKeu), request.Id);
            }

            entity.TipeLaporan = request.TipeLaporan;
            entity.TipeBaris = request.TipeBaris;
            entity.Deskripsi = request.Deskripsi;
            entity.Rumus = request.Rumus;
            entity.Level = request.Level;
            entity.Urutan = request.Urutan;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}