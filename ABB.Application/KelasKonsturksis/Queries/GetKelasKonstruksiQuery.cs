using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KelasKonsturksis.Queries
{
    public class GetKelasKonstruksiQuery : IRequest<List<KelasKonstruksiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKelasKonstruksiQueryHandler : IRequestHandler<GetKelasKonstruksiQuery, List<KelasKonstruksiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKelasKonstruksiQueryHandler> _logger;

        public GetKelasKonstruksiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKelasKonstruksiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KelasKonstruksiDto>> Handle(GetKelasKonstruksiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KelasKonstruksiDto>("SELECT * FROM rf13")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}