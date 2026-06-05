using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesPremiFakultatifMasuks.Commands
{
    public class GetProsesPremiFakultatifMasuksQuery : IRequest<List<ProsesPremiFakultatifMasukDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetProsesPremiFakultatifMasuksQueryHandler : IRequestHandler<GetProsesPremiFakultatifMasuksQuery, List<ProsesPremiFakultatifMasukDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetProsesPremiFakultatifMasuksQueryHandler> _logger;

        public GetProsesPremiFakultatifMasuksQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GetProsesPremiFakultatifMasuksQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ProsesPremiFakultatifMasukDto>> Handle(GetProsesPremiFakultatifMasuksQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<ProsesPremiFakultatifMasukDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
                    FROM uw01a p
                        INNER JOIN rf01 cb
                            ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob
                            ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob
                            ON p.kd_cob = scob.kd_cob
                            AND p.kd_scob = scob.kd_scob
                    WHERE cb.kd_cb = @KodeCabang AND st_pas = 'C' AND (p.no_aks like '%'+@SearchKeyword+'%' 
                        OR p.no_pol_pas like '%'+@SearchKeyword+'%' 
                        OR p.st_pas like '%'+@SearchKeyword+'%' 
                        OR cb.nm_cb like '%'+@SearchKeyword+'%' 
                        OR cob.nm_cob like '%'+@SearchKeyword+'%' 
                        OR scob.nm_scob like '%'+@SearchKeyword+'%' 
                        OR p.nm_ttg like '%'+@SearchKeyword+'%' 
                        OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
                }, _logger);
        }
    }
}