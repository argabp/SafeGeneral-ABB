using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class GetTemplateJurnalDetail117Query : IRequest<List<TemplateJurnalDetail117Dto>>
    {
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetTemplateJurnalDetail117QueryHandler : IRequestHandler<GetTemplateJurnalDetail117Query, List<TemplateJurnalDetail117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<GetTemplateJurnalDetail117QueryHandler> _logger;

        public GetTemplateJurnalDetail117QueryHandler(
            IDbContextPstNota context,
            ILogger<GetTemplateJurnalDetail117QueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TemplateJurnalDetail117Dto>> Handle(GetTemplateJurnalDetail117Query request,
            CancellationToken cancellationToken)
        {
            try
            {
                var reqTypeTr = request.type_tr ?? "";
                var reqTypeJr = request.type_jr ?? "";
                var reqMetode = request.metode ?? "";
                var reqEvent = request.Event ?? "";
                var reqJenis = request.jn_ass ?? "";

                var data =
                    await _context.TemplateJurnalDetail117
                        .Where(x => x.type_tr.Trim() == reqTypeTr && 
                                    x.type_jr.Trim() == reqTypeJr &&
                                    x.metode.Trim() == reqMetode &&
                                    x.Event.Trim() == reqEvent &&
                                    x.jn_ass.Trim() == reqJenis)
                        .Select(x => new TemplateJurnalDetail117Dto
                        {
                            type_tr = x.type_tr,
                            type_jr = x.type_jr,
                            metode = x.metode,
                            Event = x.Event,
                            jn_ass = x.jn_ass,
                            gl_akun = x.gl_akun,
                            gl_rumus = x.gl_rumus,
                            gl_dk = x.gl_dk,
                            gl_urut = (short)x.gl_urut,
                            flag_detail = x.flag_detail,
                            flag_nt = x.flag_nt
                        })
                        .ToListAsync(cancellationToken);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetTemplateJurnalDetail117QueryHandler");
                throw;
            }
        }
    }
}