using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetMataUangQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetMataUangQueryHandler : IRequestHandler<GetMataUangQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetMataUangQueryHandler> _logger;

        public GetMataUangQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetMataUangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetMataUangQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(@"SELECT m.kd_mtu Value, m.nm_mtu + '(' + m.symbol + ')' Text FROM rf06 m")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}