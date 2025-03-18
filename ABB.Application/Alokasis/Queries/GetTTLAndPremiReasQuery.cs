using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetTTLAndPremiReasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_share { get; set; }
        public decimal nilai_prm_reas { get; set; }
        public string kd_jns_sor { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal net_prm { get; set; }
        public decimal nilai_ttl_ptg { get; set; }
    }

    public class GetTTLAndPremiReasQueryHandler : IRequestHandler<GetTTLAndPremiReasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetTTLAndPremiReasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetTTLAndPremiReasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_ri05e_03",
                new
                {
                    request.pst_share, request.nilai_prm_reas, request.kd_jns_sor, 
                    request.nilai_prm, request.net_prm, request.nilai_ttl_ptg 
                })).FirstOrDefault();
        }
    }
}