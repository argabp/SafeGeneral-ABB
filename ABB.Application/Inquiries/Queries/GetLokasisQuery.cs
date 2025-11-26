using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Inquiries.Queries
{
    public class GetLokasisQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetLokasisQueryHandler : IRequestHandler<GetLokasisQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLokasisQueryHandler> _logger;

        public GetLokasisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLokasisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetLokasisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_lok)) Value, nm_lok Text " +
                                                                          "FROM rf19")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}