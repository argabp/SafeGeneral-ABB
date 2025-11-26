using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LKTs.Queries
{
    public class GetLKTsQuery : IRequest<List<LKTDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetLKTsQueryHandler : IRequestHandler<GetLKTsQuery, List<LKTDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLKTsQueryHandler> _logger;

        public GetLKTsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLKTsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LKTDto>> Handle(GetLKTsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<LKTDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob FROM cl09 p 
                        INNER JOIN rf01 cb
                           ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob
                           ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob
                           ON p.kd_cob = scob.kd_cob
                           AND p.kd_scob = scob.kd_scob
                        WHERE p.kd_cb = @KodeCabang AND 
                              (cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR cob.kd_cob like '%'+@SearchKeyword+'%' 
					OR scob.kd_scob like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new
                {
                    request.SearchKeyword, request.KodeCabang
                })).ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}