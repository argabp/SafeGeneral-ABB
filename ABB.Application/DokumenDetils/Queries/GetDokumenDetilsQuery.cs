using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenDetils.Queries
{
    public class GetDokumenDetilsQuery : IRequest<List<DokumenDetilDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetDokumenDetilsQueryHandler : IRequestHandler<GetDokumenDetilsQuery, List<DokumenDetilDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumenDetilsQueryHandler> _logger;

        public GetDokumenDetilsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumenDetilsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DokumenDetilDto>> Handle(GetDokumenDetilsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DokumenDetilDto>("SELECT * FROM MS_DokumenDetil")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}