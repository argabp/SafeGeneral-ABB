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
                var results = (await _connectionFactory.Query<LKTDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob,
                            c2.no_pol_lama, c.tgl_mts, c.tgl_closing FROM cl09 p 
                        INNER JOIN cl03 c
                            ON c.kd_cb = p.kd_cb
                                AND  c.kd_cob = p.kd_cob
                                AND  c.kd_scob = p.kd_scob
                                AND  c.kd_thn = p.kd_thn
                                AND  c.no_kl = p.no_kl
                                AND  c.no_mts = p.no_mts 
                        INNER JOIN cl01 c2
                            ON c.kd_cb = c2.kd_cb
                                AND  c.kd_cob = c2.kd_cob
                                AND  c.kd_scob = c2.kd_scob
                                AND  c.kd_thn = c2.kd_thn
                                AND  c.no_kl = c2.no_kl
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