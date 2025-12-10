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
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string GlAkun { get; set; }
        public string GlRumus { get; set; }
        public string GlDk { get; set; }
        public short GlUrut { get; set; }
        public string FlagDetail { get; set; }
        public bool? FlagNt { get; set; }
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
                // PERBAIKAN: Cari berdasarkan 3 kunci (Type, JenisAss, GlAkun)
                // Gunakan Trim() untuk keamanan
                var entity = _context.TemplateJurnalDetail117
                    .FirstOrDefault(w => 
                        w.Type == request.Type && 
                        w.JenisAss == request.JenisAss && 
                        w.GlAkun == request.GlAkun
                    );

                if (entity != null)
                {
                    _context.TemplateJurnalDetail117.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else 
                {
                    // Opsional: Beritahu user jika data tidak ketemu
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
