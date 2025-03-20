using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.PostingPolicies.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingKomisiTambahans.Queries
{
    public class GetPostingKomisiTambahanQuery : IRequest<List<PostingKomisiTambahanDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetPostingKomisiTambahanQueryHandler : IRequestHandler<GetPostingKomisiTambahanQuery, List<PostingKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPostingKomisiTambahanQueryHandler> _logger;

        public GetPostingKomisiTambahanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPostingKomisiTambahanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PostingKomisiTambahanDto>> Handle(GetPostingKomisiTambahanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PostingKomisiTambahanDto>("SELECT * FROM fn05")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}