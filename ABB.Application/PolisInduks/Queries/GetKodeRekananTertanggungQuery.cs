using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetKodeRekananTertanggungQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GetKodeRekananTertanggungQueryHandler : IRequestHandler<GetKodeRekananTertanggungQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKodeRekananTertanggungQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetKodeRekananTertanggungQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<string>("spe_uw02e_03c", new
                {
                    request.kd_cb,
                    request.kd_grp_rk,
                    request.kd_rk
                }))
                .First();
            return result;
        }
    }
}