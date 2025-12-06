using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABB.Application.TemplateJurnals62.Commands
{
    public class EditTemplateJurnalDetail62Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string GlAkun { get; set; }
        public string GlRumus { get; set; }
        public string GlDk { get; set; }
        public short GlUrut { get; set; }
        public string FlagDetail { get; set; }
        public bool? FlagNt { get; set; }
        
    }

    public class EditTemplateJurnalDetail62CommandHandler : IRequestHandler<EditTemplateJurnalDetail62Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<EditTemplateJurnalDetail62CommandHandler> _logger;

        public EditTemplateJurnalDetail62CommandHandler(
            IDbContextPstNota context,
            ILogger<EditTemplateJurnalDetail62CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditTemplateJurnalDetail62Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = _context.TemplateJurnalDetail62
                        .FirstOrDefault(w => w.Type == request.Type && w.JenisAss == request.JenisAss);

                    if (entity != null)
                    {
                        entity.Type = request.Type;
                        entity.JenisAss = request.JenisAss;
                        entity.GlAkun = request.GlAkun;
                        entity.GlRumus = request.GlRumus;
                        entity.GlDk = request.GlDk;
                        entity.GlUrut = request.GlUrut;
                        entity.FlagDetail = request.FlagDetail;
                        entity.FlagNt = request.FlagNt;

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
