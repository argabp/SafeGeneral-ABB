using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Pastikan ada ini buat query

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class DeleteTemplateJurnal117Command : IRequest
    {
        public string DatabaseName { get; set; }
        public string Type { get; set; }
        public string JenisAss { get; set; }
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
                var reqType = request.Type?.Trim();
                var reqJenis = request.JenisAss?.Trim();

                // -----------------------------------------------------------
                // LANGKAH 1: Hapus DATA DETAIL terlebih dahulu (Cascade Delete Manual)
                // -----------------------------------------------------------
                var details = _context.TemplateJurnalDetail117
                    .Where(d => d.Type == reqType && d.JenisAss == reqJenis)
                    .ToList();

                if (details.Any())
                {
                    _context.TemplateJurnalDetail117.RemoveRange(details);
                }

                // -----------------------------------------------------------
                // LANGKAH 2: Hapus DATA HEADER
                // -----------------------------------------------------------
                var header = _context.TemplateJurnal117
                    .FirstOrDefault(w => w.Type == reqType && w.JenisAss == reqJenis);

                if (header != null)
                {
                    _context.TemplateJurnal117.Remove(header);
                }
               
                // -----------------------------------------------------------
                // LANGKAH 3: Simpan Perubahan (Commit Transaction)
                // -----------------------------------------------------------
                // SaveChangesAsync akan mengeksekusi penghapusan Detail & Header sekaligus
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