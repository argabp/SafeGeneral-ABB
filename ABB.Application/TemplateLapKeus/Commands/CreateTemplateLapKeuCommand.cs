using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.TemplateLapKeus.Commands
{
    public class CreateTemplateLapKeuCommand : IRequest<long>
    {
        public string TipeLaporan { get; set; } // NERACA / LABARUGI
        public string TipeBaris { get; set; }   // HEADER / DETAIL / TOTAL
        public string Deskripsi { get; set; }
        public string Rumus { get; set; }
        public string Level { get; set; }
        public int Urutan { get; set; }
    }

    public class CreateTemplateLapKeuCommandHandler : IRequestHandler<CreateTemplateLapKeuCommand, long>
    {
        private readonly IDbContextPstNota _context;

        public CreateTemplateLapKeuCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreateTemplateLapKeuCommand request, CancellationToken cancellationToken)
        {
            var entity = new TemplateLapKeu
            {
                TipeLaporan = request.TipeLaporan,
                TipeBaris = request.TipeBaris,
                Deskripsi = request.Deskripsi,
                Rumus = request.Rumus,
                Level = request.Level,
                Urutan = request.Urutan,
            };

            _context.TemplateLapKeu.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}