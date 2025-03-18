using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Queries
{
    public class GetMataUangQuery : IRequest<List<MataUangDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetMataUangQueryHandler : IRequestHandler<GetMataUangQuery, List<MataUangDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetMataUangQueryHandler> _logger;

        public GetMataUangQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetMataUangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<MataUangDto>> Handle(GetMataUangQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<MataUangDto>("SELECT * FROM rf06")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}