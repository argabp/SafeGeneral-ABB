using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABB.Application.TemplateJurnals117.Commands
{
    public class EditTemplateJurnalDetail117Command : IRequest
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

    public class EditTemplateJurnalDetail117CommandHandler : IRequestHandler<EditTemplateJurnalDetail117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<EditTemplateJurnalDetail117CommandHandler> _logger;

        public EditTemplateJurnalDetail117CommandHandler(
            IDbContextPstNota context,
            ILogger<EditTemplateJurnalDetail117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditTemplateJurnalDetail117Command request, CancellationToken cancellationToken)
        {
            try
            {
                // PERBAIKAN: Tambahkan Trim() dan filter GlAkun
                var entity = _context.TemplateJurnalDetail117
                    .FirstOrDefault(w => 
                        w.Type == request.Type && 
                        w.JenisAss == request.JenisAss && 
                        w.GlAkun == request.GlAkun // <--- INI WAJIB ADA
                    );

                if (entity != null)
                {
                    // Jangan update Primary Key (Type, JenisAss, GlAkun)
                    // entity.Type = request.Type; // Tidak perlu update key
                    // entity.JenisAss = request.JenisAss; // Tidak perlu update key
                    // entity.GlAkun = request.GlAkun; // Tidak perlu update key

                    entity.GlRumus = request.GlRumus;
                    entity.GlDk = request.GlDk;
                    entity.GlUrut = request.GlUrut;
                    entity.FlagDetail = request.FlagDetail;
                    entity.FlagNt = request.FlagNt; // Sesuaikan tipe data FlagNt di DB (biasanya string Y/N atau bit)

                    await _context.SaveChangesAsync(cancellationToken);
                }
                else 
                {
                    throw new Exception($"Data tidak ditemukan. Akun: {request.GlAkun}");
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
