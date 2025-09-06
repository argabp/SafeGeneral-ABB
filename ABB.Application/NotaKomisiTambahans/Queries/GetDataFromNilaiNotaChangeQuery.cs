using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetDataFromNilaiNotaChangeQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string jns_nt_kel { get; set; }
        public string kd_grp_ttj { get; set; }
        public decimal nilai_nt { get; set; }
        public string uraian { get; set; }
        public decimal nilai_prm { get; set; }
        public string no_pol_ttg { get; set; }
        public string kd_mtu { get; set; }
        public string kd_cb { get; set; }
        public string kd_rk_ttj { get; set; }
    }

    public class GetDataFromNilaiNotaChangeQueryHandler : IRequestHandler<GetDataFromNilaiNotaChangeQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetDataFromNilaiNotaChangeQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GetDataFromNilaiNotaChangeQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_07",
                new
                {
                    request.kd_mtu, request.nilai_prm, request.no_pol_ttg, 
                    request.nilai_nt, request.jns_nt_kel, request.kd_grp_ttj, 
                    request.uraian, request.kd_cb, request.kd_rk_ttj 
                })).ToList();
        }
    }
}