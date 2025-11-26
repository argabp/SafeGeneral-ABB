using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryPranotaKoassQuery : IRequest<List<InquiryPranotaKoasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetInquiryPranotaKoassQueryHandler : IRequestHandler<GetInquiryPranotaKoassQuery, List<InquiryPranotaKoasDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetInquiryPranotaKoassQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InquiryPranotaKoasDto>> Handle(GetInquiryPranotaKoassQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<InquiryPranotaKoasDto>(@"SELECT r.nm_rk, p.*, p.kd_grp_pas + p.kd_rk_pas Id
				FROM uw03e p
					INNER JOIN rf03 r
						ON p.kd_cb = r.kd_cb
				    		AND p.kd_grp_pas = r.kd_grp_rk
							AND p.kd_rk_pas = r.kd_rk
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_pol = @no_pol AND 
				      p.no_updt = @no_updt AND
				      p.kd_mtu = @kd_mtu", 
	            new { request.SearchKeyword, request.KodeCabang, 
		            request.kd_cob, request.kd_scob, request.kd_thn,
		            request.no_pol, request.no_updt, request.kd_mtu
	            })).ToList();
        }
    }
}