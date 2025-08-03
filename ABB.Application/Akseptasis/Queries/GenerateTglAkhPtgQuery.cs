using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateTglAkhPtgQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal jk_wkt_main { get; set; }
        public DateTime tgl_akh_ptg { get; set; }
    }

    public class GenerateTglAkhPtgQueryHandler : IRequestHandler<GenerateTglAkhPtgQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateTglAkhPtgQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateTglAkhPtgQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_92", new { request.jk_wkt_main, request.tgl_akh_ptg })).FirstOrDefault();
        }
    }
}