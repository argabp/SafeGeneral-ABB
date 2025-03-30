using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingPolis.Queries
{
    public class GetCancelPostingPolisQuery : IRequest<List<CancelPostingPolisDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetCancelPostingPolisQueryHandler : IRequestHandler<GetCancelPostingPolisQuery, List<CancelPostingPolisDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCancelPostingPolisQueryHandler> _logger;

        public GetCancelPostingPolisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCancelPostingPolisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CancelPostingPolisDto>> Handle(GetCancelPostingPolisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CancelPostingPolisDto>("SELECT * FROM v_uw08e WHERE flag_posting = 'Y'")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}