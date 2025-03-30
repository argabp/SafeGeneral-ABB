using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNilaiPPHAndPPN : IRequest<string>
    {
        public string DatabaseName { get; set; }
        
        public decimal nilai_nt { get; set; }
        
        public string uraian { get; set; }
        
        public decimal pst_pph { get; set; }
        
        public decimal pst_ppn { get; set; }
    }

    public class GetNilaiPPHAndPPNHandler : IRequestHandler<GetNilaiPPHAndPPN, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNilaiPPHAndPPNHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetNilaiPPHAndPPN request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw06e_08",
                new
                {
                    request.nilai_nt, request.uraian,
                    request.pst_pph, request.pst_ppn
                })).FirstOrDefault();
        }
    }
}