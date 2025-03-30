using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetPercentageQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string jns_nt_kel { get; set; }
        public string kd_grp_ttj { get; set; }
        public decimal nilai_nt { get; set; }
        public string kd_rk_ttj { get; set; }
    }

    public class GetPercentageQueryHandler : IRequestHandler<GetPercentageQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetPercentageQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GetPercentageQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_10",
                new
                {
                    request.jns_nt_kel, request.kd_grp_ttj, 
                    request.nilai_nt, request.kd_rk_ttj 
                })).ToList();
        }
    }
}