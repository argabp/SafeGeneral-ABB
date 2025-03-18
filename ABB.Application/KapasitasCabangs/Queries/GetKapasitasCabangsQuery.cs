using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KapasitasCabangs.Queries
{
    public class GetKapasitasCabangsQuery : IRequest<List<KapasitasCabangDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKapasitasCabangsQueryHandler : IRequestHandler<GetKapasitasCabangsQuery, List<KapasitasCabangDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKapasitasCabangsQueryHandler> _logger;

        public GetKapasitasCabangsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKapasitasCabangsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KapasitasCabangDto>> Handle(GetKapasitasCabangsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KapasitasCabangDto>(@"SELECT RTRIM(LTRIM(b.kd_cb)) + RTRIM(LTRIM(b.kd_cob)) + RTRIM(LTRIM(b.kd_scob)) + CONVERT(varchar,b.thn) Id ,
                                                                        b.*, ca.nm_cb,
																		c.nm_cob, s.nm_scob FROM rf43 b 
																		INNER JOIN rf04 c ON b.kd_cob = c.kd_cob
																		INNER JOIN rf01 ca ON b.kd_cb = ca.kd_cb
																		INNER JOIN rf05 s on s.kd_scob = b.kd_scob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}