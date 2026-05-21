using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class DeleteTemplateJurnal117Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
    }

    public class DeleteTemplateJurnal117CommandHandler : IRequestHandler<DeleteTemplateJurnal117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<DeleteTemplateJurnal117CommandHandler> _logger;

        public DeleteTemplateJurnal117CommandHandler(
            IDbContextPstNota context,
            ILogger<DeleteTemplateJurnal117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTemplateJurnal117Command request, CancellationToken cancellationToken)
        {
            try
            {
                var reqTypeTr = request.type_tr?.Trim();
                var reqTypeJr = request.type_jr?.Trim();
                var reqMetode = request.metode?.Trim();
                var reqEvent = request.Event?.Trim();
                var reqJenisAss = request.jn_ass?.Trim();

                // 1. Hapus DATA DETAIL terlebih dahulu
                var details = _context.TemplateJurnalDetail117
                    .Where(d => d.type_tr == reqTypeTr && 
                                d.type_jr == reqTypeJr && 
                                d.metode == reqMetode && 
                                d.Event == reqEvent && 
                                d.jn_ass == reqJenisAss)
                    .ToList();

                if (details.Any())
                {
                    _context.TemplateJurnalDetail117.RemoveRange(details);
                }

                // 2. Hapus DATA HEADER
                var header = _context.TemplateJurnal117
                    .FirstOrDefault(w => w.type_tr == reqTypeTr && 
                                         w.type_jr == reqTypeJr && 
                                         w.metode == reqMetode && 
                                         w.Event == reqEvent && 
                                         w.jn_ass == reqJenisAss);

                if (header != null)
                {
                    _context.TemplateJurnal117.Remove(header);
                }
                
                // 3. Simpan Perubahan
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