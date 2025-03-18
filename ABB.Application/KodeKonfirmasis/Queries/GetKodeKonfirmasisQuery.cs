using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.KodeKonfirmasis.Queries
{
    public class GetKodeKonfirmasisQuery : IRequest<List<KodeKonfirmasiDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetKodeKonfirmasisQueryHandler : IRequestHandler<GetKodeKonfirmasisQuery, List<KodeKonfirmasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKodeKonfirmasisQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<KodeKonfirmasiDto>> Handle(GetKodeKonfirmasisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<KodeKonfirmasiDto>(@"SELECT r.kd_cb + r.no_aks + r.kd_konfirm + r.kd_cob + r.kd_scob + r.kd_thn Id,
                r.kd_cb, c.nm_cb, r.no_aks, r.kd_konfirm, r.kd_cob, r.kd_scob, r.kd_thn
                    FROM rf42 r
                INNER JOIN rf01 c
                    ON r.kd_cb = c.kd_cb
                WHERE c.kd_cb = @KodeCabang", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}