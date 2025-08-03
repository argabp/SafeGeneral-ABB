using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstRateTRSQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_casco { get; set; }
        public bool flag_trs { get; set; }
    }

    public class GeneratePstRateTRSQueryHandler : IRequestHandler<GeneratePstRateTRSQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GeneratePstRateTRSQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GeneratePstRateTRSQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw09e01_05", 
                new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_casco, request.flag_trs })).FirstOrDefault();
        }
    }
}