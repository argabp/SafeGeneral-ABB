using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EntriNotas.Queries
{
    public class GenerateNilaiAngQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_ang { get; set; }
        public decimal nilai_nt { get; set; }
        public decimal nilai_ppn { get; set; }
        public decimal nilai_pph { get; set; }
    }

    public class GenerateNilaiAngQueryHandler : IRequestHandler<GenerateNilaiAngQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiAngQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiAngQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_02", 
                new { request.pst_ang, request.nilai_nt, request.nilai_ppn, request.nilai_pph })).FirstOrDefault();
        }
    }
}