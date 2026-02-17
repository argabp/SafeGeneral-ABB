using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.AkseptasiProduks.Queries
{
    public class GetAkseptasiProduksQuery : IRequest<List<AkseptasiProdukDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetAkseptasiProduksQueryHandler : IRequestHandler<GetAkseptasiProduksQuery, List<AkseptasiProdukDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkseptasiProduksQueryHandler> _logger;

        public GetAkseptasiProduksQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAkseptasiProduksQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<AkseptasiProdukDto>> Handle(GetAkseptasiProduksQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<AkseptasiProdukDto>(@"SELECT p.*, cob.nm_cob, scob.nm_scob
				FROM MS_AkseptasiProduk p
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_scob = scob.kd_scob
				WHERE (p.Desc_AkseptasiProduk like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();

                var sequence = 0;
                foreach (var result in results)
                {
                    sequence++;
                    result.Id = sequence;
                }

                return results;
            }, _logger);
        }
    }
}