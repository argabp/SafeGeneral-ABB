using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetValidationQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string jns_nt_kel { get; set; }
        public string kd_grp_ttj { get; set; }
        public decimal nilai_nt { get; set; }
        public string no_nt_msk { get; set; }
        public decimal nilai_prm { get; set; }
        public string no_pol_ttj { get; set; }
        public string kd_mtu { get; set; }
        public decimal pst_nt { get; set; }
        public string no_ref { get; set; }
        public string kd_rrk_ttj { get; set; }
    }

    public class GetValidationQueryHandler : IRequestHandler<GetValidationQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetValidationQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetValidationQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_07_01",
                new
                {
                    request.jns_nt_kel, request.kd_grp_ttj, request.nilai_nt,
                    request.nilai_prm, request.no_pol_ttj, request.kd_mtu, 
                    request.no_nt_msk, request.pst_nt, request.no_ref,
                    request.kd_rrk_ttj
                })).FirstOrDefault();
        }
    }
}