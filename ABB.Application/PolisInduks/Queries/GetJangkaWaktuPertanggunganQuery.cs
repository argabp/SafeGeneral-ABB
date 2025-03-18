using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetJangkaWaktuPertanggunganQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public DateTime tgl_mul_ptg { get; set; }
        public DateTime tgl_akh_ptg { get; set; }
        public string kd_cob { get; set; }
    }

    public class GetJangkaWaktuPertanggunganQueryHandler : IRequestHandler<GetJangkaWaktuPertanggunganQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetJangkaWaktuPertanggunganQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetJangkaWaktuPertanggunganQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var jk_wkt_ptg = (await _connectionFactory.QueryProc<string>("spe_uw02e_28", new
                {
                    request.tgl_mul_ptg,
                    request.tgl_akh_ptg,
                    request.kd_cob
                }))
                .First().Split(",")[1];
            return jk_wkt_ptg;
        }
    }
}