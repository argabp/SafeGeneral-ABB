using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetTertanggungDataQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public string kd_mtu_1 { get; set; }
    }

    public class GetTertanggungDataQueryHandler : IRequestHandler<GetTertanggungDataQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetTertanggungDataQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<string>> Handle(GetTertanggungDataQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw06e_04",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, 
                    request.kd_thn, request.no_pol, request.no_updt, 
                    request.kd_mtu_1
                })).ToList();
        }
    }
}