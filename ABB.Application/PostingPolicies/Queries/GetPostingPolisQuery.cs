using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingPolicies.Queries
{
    public class GetPostingPolisQuery : IRequest<List<PostingPolisDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetPostingPolisQueryHandler : IRequestHandler<GetPostingPolisQuery, List<PostingPolisDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPostingPolisQueryHandler> _logger;

        public GetPostingPolisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPostingPolisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PostingPolisDto>> Handle(GetPostingPolisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PostingPolisDto>("SELECT * FROM v_uw08e")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}