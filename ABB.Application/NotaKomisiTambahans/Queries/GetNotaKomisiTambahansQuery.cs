using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNotaKomisiTambahansQuery : IRequest<List<NotaKomisiTambahanDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }
        
        public string kd_endt { get; set; }
    }

    public class GetNotaKomisiTambahansQueryHandler : IRequestHandler<GetNotaKomisiTambahansQuery, List<NotaKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNotaKomisiTambahansQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<NotaKomisiTambahanDto>> Handle(GetNotaKomisiTambahansQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<NotaKomisiTambahanDto>(@"SELECT 
    			p.*, '(' + p.kd_cb + ') ' + cb.nm_cb nm_cb,
    			'(' + p.kd_cob + ') ' + cob.nm_cob nm_cob,
    			'(' + p.kd_scob + ') ' + scob.nm_scob nm_scob,
    			p.kd_cb + ' ' + p.kd_cob + ' ' + p.kd_scob + ' ' +
				p.kd_thn + ' ' + p.no_pol + ' ' + CONVERT(varchar, p.no_updt) no_akseptasi FROM fn05 p
    			    INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_scob = scob.kd_scob
				WHERE (p.no_pol like '%'+@SearchKeyword+'%' 
					OR p.jns_tr like '%'+@SearchKeyword+'%' 
					OR p.jns_nt_msk like '%'+@SearchKeyword+'%' 
					OR p.kd_thn like '%'+@SearchKeyword+'%' 
					OR p.kd_bln like '%'+@SearchKeyword+'%' 
					OR p.no_nt_msk like '%'+@SearchKeyword+'%' 
					OR p.jns_nt_kel like '%'+@SearchKeyword+'%' 
					OR p.no_nt_kel like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", 
                new { request.SearchKeyword, request.kd_cb,
                })).ToList();
        }
    }
}