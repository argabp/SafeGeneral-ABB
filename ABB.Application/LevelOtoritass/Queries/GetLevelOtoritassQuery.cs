using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LevelOtoritass.Queries
{
    public class GetLevelOtoritassQuery : IRequest<List<LevelOtoritasDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetLevelOtoritassQueryHandler : IRequestHandler<GetLevelOtoritassQuery, List<LevelOtoritasDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLevelOtoritassQueryHandler> _logger;

        public GetLevelOtoritassQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLevelOtoritassQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LevelOtoritasDto>> Handle(GetLevelOtoritassQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LevelOtoritasDto>(@"SELECT * FROM rf41")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}