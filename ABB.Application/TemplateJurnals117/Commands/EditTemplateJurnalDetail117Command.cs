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
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string gl_akun { get; set; }
        public string gl_rumus { get; set; }
        public string gl_dk { get; set; }
        public short gl_urut { get; set; }
        public string flag_detail { get; set; }
        public bool? flag_nt { get; set; }
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
                    entity.gl_rumus = request.gl_rumus;
                    entity.gl_dk = request.gl_dk;
                    entity.gl_urut = request.gl_urut;
                    entity.flag_detail = request.flag_detail;
                    entity.flag_nt = request.flag_nt; 

                    await _context.SaveChangesAsync(cancellationToken);
                }
                else 
                {
                    throw new Exception($"Data tidak ditemukan. Akun: {request.gl_akun}");
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