using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateDataKapalQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }
    }

    public class GenerateDataKapalQueryHandler : IRequestHandler<GenerateDataKapalQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateDataKapalQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GenerateDataKapalQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_03b", new { request.kd_kapal })).ToList();
        }
    }
}