using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstRatePADQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_pad { get; set; }
    }

    public class GeneratePstRatePADQueryHandler : IRequestHandler<GeneratePstRatePADQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GeneratePstRatePADQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GeneratePstRatePADQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw09e01_12", 
                new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_pad })).FirstOrDefault();
        }
    }
}