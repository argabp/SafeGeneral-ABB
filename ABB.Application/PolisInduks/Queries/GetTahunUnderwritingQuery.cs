using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetTahunUnderwritingQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public DateTime tgl_mul_ptg { get; set; }
    }

    public class GetTahunUnderwritingQueryHandler : IRequestHandler<GetTahunUnderwritingQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetTahunUnderwritingQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetTahunUnderwritingQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var thn_underwriting = (await _connectionFactory.QueryProc<string>("spe_uw02e_15", new { request.tgl_mul_ptg }))
                .Last().Split(",")[1];
            return thn_underwriting;
        }
    }
}