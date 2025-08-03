using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmBtnQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_prm_std { get; set; }
        public decimal nilai_prm_bjr { get; set; }
        public decimal nilai_prm_tl { get; set; }
        public decimal nilai_prm_gb { get; set; }
        public decimal nilai_prm_phk { get; set; }
        public decimal nilai_bia_adm { get; set; }
        public decimal nilai_bia_mat { get; set; }
        public int jk_wkt { get; set; }
        public string flag_bln { get; set; }
    }

    public class GenerateNilaiPrmBtnQueryHandler : IRequestHandler<GenerateNilaiPrmBtnQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiPrmBtnQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiPrmBtnQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_kp02e_06_srm", 
                new
                {
                    request.nilai_prm_std, request.nilai_prm_bjr, request.nilai_prm_tl,
                    request.nilai_prm_gb, request.nilai_prm_phk, request.nilai_bia_adm,
                    request.nilai_bia_mat, request.jk_wkt, request.flag_bln
                })).FirstOrDefault();
        }
    }
}