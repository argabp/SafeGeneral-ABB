using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetGroupAndRekananSorQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_cob { get; set; }
        public string kd_cb { get; set; }
        public decimal thn_uw { get; set; }
        public decimal nilai_ttl_ptg { get; set; }
        public decimal nilai_prm { get; set; }
    }

    public class GetGroupAndRekananSorQueryHandler : IRequestHandler<GetGroupAndRekananSorQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetGroupAndRekananSorQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetGroupAndRekananSorQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_ri05e_02",
                new
                {
                    request.kd_jns_sor, request.kd_cob, request.kd_cb,
                    request.thn_uw, request.nilai_ttl_ptg, request.nilai_prm
                })).FirstOrDefault();
        }
    }
}