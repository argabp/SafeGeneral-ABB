using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetPstShareAndNilaiPremiReasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_grp_ttj { get; set; }
        public string kd_rk_ttj { get; set; }
    }

    public class GetPstShareAndNilaiPremiReasQueryHandler : IRequestHandler<GetPstShareAndNilaiPremiReasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetPstShareAndNilaiPremiReasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetPstShareAndNilaiPremiReasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_01",
                new
                {
                    kd_grp_rk = request.kd_grp_ttj, request.kd_cb, kd_rk = request.kd_rk_ttj 
                })).FirstOrDefault();
        }
    }
}