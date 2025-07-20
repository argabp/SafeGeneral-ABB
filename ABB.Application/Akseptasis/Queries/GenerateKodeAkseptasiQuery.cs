using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateKodeAkseptasiQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string st_pas { get; set; }
        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
    }

    public class GenerateKodeAkseptasiQueryHandler : IRequestHandler<GenerateKodeAkseptasiQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateKodeAkseptasiQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GenerateKodeAkseptasiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_34", new { request.st_pas, request.kd_grp_sb_bis, request.kd_rk_sb_bis })).ToList();
        }
    }
}