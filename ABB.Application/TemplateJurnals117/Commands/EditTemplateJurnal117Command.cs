using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class EditTemplateJurnal117Command : IRequest
    {
        public string DatabaseName { get; set; }

        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string NamaJurnal { get; set; }
        
    }

    public class EditTemplateJurnal117CommandHandler : IRequestHandler<EditTemplateJurnal117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<EditTemplateJurnal117CommandHandler> _logger;

        public EditTemplateJurnal117CommandHandler(
            IDbContextPstNota context,
            ILogger<EditTemplateJurnal117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditTemplateJurnal117Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = _context.TemplateJurnal117
                        .FirstOrDefault(w => w.Type == request.Type && w.JenisAss == request.JenisAss);

                    if (entity != null)
                    {
                        entity.Type = request.Type;
                        entity.JenisAss = request.JenisAss;
                        entity.NamaJurnal = request.NamaJurnal;

                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }

                return Unit.Value;
            }
    }
}
