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
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string nm_jr { get; set; }
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
                // Primary Key tidak boleh diubah, kita jadikan acuan pencarian
                var entity = _context.TemplateJurnal117
                    .FirstOrDefault(w => 
                        w.type_tr == request.type_tr && 
                        w.type_jr == request.type_jr && 
                        w.metode == request.metode && 
                        w.Event == request.Event && 
                        w.jn_ass == request.jn_ass
                    );

                if (entity != null)
                {
                    entity.nm_jr = request.nm_jr;
                    
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