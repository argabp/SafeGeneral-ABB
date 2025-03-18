using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetKmsReasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_kms_reas { get; set; }
        public decimal nilai_prm_reas { get; set; }
        public decimal nilai_adj_reas { get; set; }
    }

    public class GetKmsReasQueryHandler : IRequestHandler<GetKmsReasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKmsReasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetKmsReasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_ri05e_04",
                new
                {
                    request.pst_kms_reas, request.nilai_prm_reas, request.nilai_adj_reas
                })).FirstOrDefault();
        }
    }
}