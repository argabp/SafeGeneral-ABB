using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNilaiAndPercentageQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }
        public DateTime tgl_nt { get; set; }
        public decimal pst_nt { get; set; }
        public decimal nilai_nt { get; set; }
        public string jns_nt_kel { get; set; }
        public string kd_grp_ttj { get; set; }
        public string uraian { get; set; }
        public string kd_cb { get; set; }
        public string kd_rk_ttj { get; set; }
    }

    public class GetNilaiAndPercentageQueryHandler : IRequestHandler<GetNilaiAndPercentageQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNilaiAndPercentageQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GetNilaiAndPercentageQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw06e_07",
                new
                {
                    request.kd_mtu, request.tgl_nt, request.pst_nt, 
                    request.nilai_nt, request.jns_nt_kel, request.kd_grp_ttj, 
                    request.uraian, request.kd_cb, request.kd_rk_ttj 
                })).ToList();
        }
    }
}