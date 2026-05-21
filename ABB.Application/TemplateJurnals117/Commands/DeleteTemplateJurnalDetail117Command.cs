using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class DeleteTemplateJurnalDetail117Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string gl_akun { get; set; }
    }

    public class DeleteTemplateJurnalDetail117CommandHandler : IRequestHandler<DeleteTemplateJurnalDetail117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<DeleteTemplateJurnalDetail117CommandHandler> _logger;

        public DeleteTemplateJurnalDetail117CommandHandler(
            IDbContextPstNota context,
            ILogger<DeleteTemplateJurnalDetail117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTemplateJurnalDetail117Command request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _context.TemplateJurnalDetail117
                    .FirstOrDefault(w => 
                        w.type_tr == request.type_tr && 
                        w.type_jr == request.type_jr && 
                        w.metode == request.metode && 
                        w.Event == request.Event && 
                        w.jn_ass == request.jn_ass && 
                        w.gl_akun == request.gl_akun
                    );

                if (entity != null)
                {
                    _context.TemplateJurnalDetail117.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else 
                {
                    throw new Exception("Data tidak ditemukan atau sudah dihapus.");
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