using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SebabKejadians.Queries
{
    public class GetSebabKejadiansQuery : IRequest<List<SebabKejadianDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetSebabKejadiansQueryHandler : IRequestHandler<GetSebabKejadiansQuery, List<SebabKejadianDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetSebabKejadiansQueryHandler> _logger;

        public GetSebabKejadiansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetSebabKejadiansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<SebabKejadianDto>> Handle(GetSebabKejadiansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<SebabKejadianDto>(@"
                    SELECT s.kd_cob + s.kd_sebab Id,
	                    s.kd_cob, c.nm_cob, s.kd_sebab,
	                    s.nm_sebab FROM rf34 s
	                    INNER JOIN rf04 c
		                    ON s.kd_cob = c.kd_cob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}