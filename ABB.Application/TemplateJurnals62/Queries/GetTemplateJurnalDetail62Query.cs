using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TemplateJurnals62.Queries
{
    public class GetTemplateJurnalDetail62Query : IRequest<List<TemplateJurnalDetail62Dto>>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
    }

    public class GetTemplateJurnalDetail62QueryHandler : IRequestHandler<GetTemplateJurnalDetail62Query, List<TemplateJurnalDetail62Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly ILogger<GetTemplateJurnalDetail62QueryHandler> _logger;

        public GetTemplateJurnalDetail62QueryHandler(
            IDbContextPstNota context,
            ILogger<GetTemplateJurnalDetail62QueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TemplateJurnalDetail62Dto>> Handle(GetTemplateJurnalDetail62Query request,
            CancellationToken cancellationToken)
        {
            try
            {
                var data =
                    await _context.TemplateJurnalDetail62
                        .Where(x => x.Type == request.Type &&
                                    x.JenisAss == request.JenisAss)
                        .Select(x => new TemplateJurnalDetail62Dto
                        {
                            Type = x.Type,
                            JenisAss = x.JenisAss,
                            GlAkun = x.GlAkun,
                            GlRumus = x.GlRumus,
                            GlDk = x.GlDk,
                            GlUrut = x.GlUrut,
                            FlagDetail = x.FlagDetail,
                            FlagNt = x.FlagNt
                        })
                        .ToListAsync(cancellationToken);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetTemplateJurnalDetail62QueryHandler");
                throw;
            }
        }
    }
}
