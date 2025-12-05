using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABB.Application.TemplateJurnals62.Commands
{
    public class DeleteTemplateJurnalDetail62Command : IRequest
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

    public class DeleteTemplateJurnalDetail62CommandHandler : IRequestHandler<DeleteTemplateJurnalDetail62Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<DeleteTemplateJurnalDetail62CommandHandler> _logger;

        public DeleteTemplateJurnalDetail62CommandHandler(
            IDbContextPstNota context,
            ILogger<DeleteTemplateJurnalDetail62CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTemplateJurnalDetail62Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = _context.TemplateJurnalDetail62
                        .FirstOrDefault(w => w.GlAkun == request.GlAkun);

                    if (entity != null)
                    {
                        _context.TemplateJurnalDetail62.Remove(entity);
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
