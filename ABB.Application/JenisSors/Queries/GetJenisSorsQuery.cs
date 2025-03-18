using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.JenisSors.Queries
{
    public class GetJenisSorsQuery : IRequest<List<JenisSorDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetJenisSorsQueryHandler : IRequestHandler<GetJenisSorsQuery, List<JenisSorDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetJenisSorsQueryHandler> _logger;

        public GetJenisSorsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetJenisSorsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<JenisSorDto>> Handle(GetJenisSorsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<JenisSorDto>("SELECT * FROM rf18")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}