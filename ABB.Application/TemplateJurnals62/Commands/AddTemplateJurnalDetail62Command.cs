using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using ABB.Domain.Entities;

namespace ABB.Application.TemplateJurnals62.Commands
{
    public class AddTemplateJurnalDetail62Command : IRequest
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

    public class AddTemplateJurnalDetail62CommandHandler : IRequestHandler<AddTemplateJurnalDetail62Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<AddTemplateJurnalDetail62CommandHandler> _logger;

        public AddTemplateJurnalDetail62CommandHandler(
            IDbContextPstNota context,
            ILogger<AddTemplateJurnalDetail62CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddTemplateJurnalDetail62Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = new TemplateJurnalDetail62
                    {
                        Type = request.Type,
                        JenisAss = request.JenisAss,
                        GlAkun = request.GlAkun,
                        GlRumus = request.GlRumus,
                        GlDk = request.GlDk,
                        GlUrut = request.GlUrut,
                        FlagDetail = request.FlagDetail,
                        FlagNt = request.FlagNt
                    };

                    _context.TemplateJurnalDetail62.Add(entity);
                    await _context.SaveChangesAsync(cancellationToken);
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
