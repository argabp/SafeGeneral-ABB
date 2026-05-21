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
    public class AddTemplateJurnalDetail117Command : IRequest
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

    public class AddTemplateJurnalDetail117CommandHandler : IRequestHandler<AddTemplateJurnalDetail117Command>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<AddTemplateJurnalDetail117CommandHandler> _logger;

        public AddTemplateJurnalDetail117CommandHandler(
            IDbContextPstNota context,
            ILogger<AddTemplateJurnalDetail117CommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddTemplateJurnalDetail117Command request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new TemplateJurnalDetail117
                {
                    type_tr = request.type_tr,
                    type_jr = request.type_jr,
                    metode = request.metode,
                    Event = request.Event,
                    jn_ass = request.jn_ass,
                    gl_akun = request.gl_akun,
                    gl_rumus = request.gl_rumus,
                    gl_dk = request.gl_dk,
                    gl_urut = request.gl_urut,
                    flag_detail = request.flag_detail,
                    flag_nt = request.flag_nt
                };

                _context.TemplateJurnalDetail117.Add(entity);
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