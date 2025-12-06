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
    public class AddTemplateJurnal62Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string NamaJurnal { get; set; }
    }

    public class AddTemplateJurnal62CommandHandler : IRequestHandler<AddTemplateJurnal62Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<AddTemplateJurnal62CommandHandler> _logger;

        public AddTemplateJurnal62CommandHandler(
            IDbContextPstNota context,
            ILogger<AddTemplateJurnal62CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddTemplateJurnal62Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = new TemplateJurnal62
                    {
                        Type = request.Type,
                        JenisAss = request.JenisAss,
                        NamaJurnal = request.NamaJurnal
                    };

                    _context.TemplateJurnal62.Add(entity);
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
