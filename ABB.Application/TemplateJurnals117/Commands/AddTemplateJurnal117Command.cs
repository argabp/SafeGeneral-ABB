using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using ABB.Domain.Entities;

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class AddTemplateJurnal117Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string nm_jr { get; set; }
    }

    public class AddTemplateJurnal117CommandHandler : IRequestHandler<AddTemplateJurnal117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<AddTemplateJurnal117CommandHandler> _logger;

        public AddTemplateJurnal117CommandHandler(
            IDbContextPstNota context,
            ILogger<AddTemplateJurnal117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddTemplateJurnal117Command request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new TemplateJurnal117
                {
                    type_tr = request.type_tr,
                    type_jr = request.type_jr,
                    metode = request.metode,
                    Event = request.Event,
                    jn_ass = request.jn_ass,
                    nm_jr = request.nm_jr
                };

                _context.TemplateJurnal117.Add(entity);
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