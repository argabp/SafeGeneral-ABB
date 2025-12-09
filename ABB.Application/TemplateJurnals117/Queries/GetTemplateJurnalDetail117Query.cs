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
        public string Type { get; set; }
        public string JenisAss { get; set; }
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
                var reqType = request.Type?.Trim() ?? "";
                var reqJenis = request.JenisAss?.Trim() ?? "";

                
                var data =
                    await _context.TemplateJurnalDetail117
                        .Where(x => x.Type.Trim() == reqType && 
                            x.JenisAss.Trim() == reqJenis)
                        .Select(x => new TemplateJurnalDetail117Dto
                        {
                            Type = x.Type,
                            JenisAss = x.JenisAss,
                            GlAkun = x.GlAkun,
                            GlRumus = x.GlRumus,
                            GlDk = x.GlDk,
                            GlUrut = (int)x.GlUrut,
                            FlagDetail = x.FlagDetail,
                            FlagNt = x.FlagNt
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
