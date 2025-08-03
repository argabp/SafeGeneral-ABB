using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateOtherPAPremiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public byte stn_rate { get; set; }
        public string kd_grp_kr { get; set; }
        public string flag { get; set; }
        public decimal nilai_harga_ptg { get; set; }
        public decimal pst_rate { get; set; }

        public int no { get; set; }
    }

    public class GenerateOtherPAPremiQueryHandler : IRequestHandler<GenerateOtherPAPremiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateOtherPAPremiQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateOtherPAPremiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_kp02e_05", 
                new
                {
                    request.stn_rate, kd_rms = request.kd_grp_kr, request.flag,
                    request.nilai_harga_ptg, request.pst_rate, request.no
                })).FirstOrDefault();
        }
    }
}