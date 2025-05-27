using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class GenerateKeteranganResikoQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
    }

    public class GenerateKeteranganResikoQueryHandler : IRequestHandler<GenerateKeteranganResikoQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateKeteranganResikoQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateKeteranganResikoQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("sp_GenerateKetRisiko",
                new
                {
                    request.kd_cob, request.kd_scob
                })).First();
        }
    }
}