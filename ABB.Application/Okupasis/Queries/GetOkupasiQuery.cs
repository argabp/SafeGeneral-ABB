using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Queries
{
    public class GetOkupasiQuery : IRequest<List<OkupasiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetOkupasiQueryHandler : IRequestHandler<GetOkupasiQuery, List<OkupasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetOkupasiQueryHandler> _logger;

        public GetOkupasiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetOkupasiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<OkupasiDto>> Handle(GetOkupasiQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var okupasis = (await _connectionFactory.Query<OkupasiDto>("SELECT * FROM rf11")).ToList();

                foreach (var okupasi in okupasis)
                    okupasi.kd_okup = okupasi.kd_okup.Trim();

                return okupasis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}