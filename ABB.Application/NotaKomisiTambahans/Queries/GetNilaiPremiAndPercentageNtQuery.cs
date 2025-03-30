using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNilaiPremiAndPercentageNtQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public string kd_mtu { get; set; }
        public decimal nilai_nt { get; set; }
    }

    public class GetNilaiPremiAndPercentageNtQueryHandler : IRequestHandler<GetNilaiPremiAndPercentageNtQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNilaiPremiAndPercentageNtQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetNilaiPremiAndPercentageNtQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw06e_06",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, 
                    request.kd_thn, request.no_pol, request.no_updt, 
                    request.kd_mtu, request.nilai_nt
                })).FirstOrDefault();
        }
    }
}