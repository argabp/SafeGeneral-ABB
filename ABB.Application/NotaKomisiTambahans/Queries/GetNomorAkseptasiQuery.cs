using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Application.Navigations.Queries;
using ABB.Application.NotaKomisiTambahans.Commands;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNomorAkseptasiQuery : IRequest<List<NomorAkseptasiDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetNomorAkseptasiQueryHandler : IRequestHandler<GetNomorAkseptasiQuery, List<NomorAkseptasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNomorAkseptasiQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<NomorAkseptasiDto>> Handle(GetNomorAkseptasiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var datas = (await _connectionFactory.Query<NomorAkseptasiDto>(@"SELECT 
    		p.*, '(' + p.kd_cb + ') ' + cb.nm_cb nm_cb,
    		'(' + p.kd_cob + ') ' + cob.nm_cob nm_cob,
    		'(' + p.kd_scob + ') ' + scob.nm_scob nm_scob,
    		p.kd_cb + '.' + p.kd_cob + '.' + p.kd_scob + '.' +
			p.kd_thn + '.' + p.no_pol + '.' + CONVERT(varchar, p.no_updt) no_akseptasi FROM  v_uw06_01 p
    		    INNER JOIN rf01 cb
					ON p.kd_cb = cb.kd_cb 
				INNER JOIN rf04 cob
					ON p.kd_cob = cob.kd_cob
				INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob")).ToList();

            var id = 0;
            foreach (var data in datas)
            {
                id++;
                data.Id = id;
            }

            return datas;
        }
    }
}