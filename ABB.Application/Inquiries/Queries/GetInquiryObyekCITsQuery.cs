using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryObyekCITsQuery : IRequest<List<InquiryObyekCITDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }
        
        public string kd_endt { get; set; }
    }

    public class GetInquiryObyekCITsQueryHandler : IRequestHandler<GetInquiryObyekCITsQuery, List<InquiryObyekCITDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetInquiryObyekCITsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InquiryObyekCITDto>> Handle(GetInquiryObyekCITsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<InquiryObyekCITDto>(@"SELECT p.*, r.nm_lok nm_asal 
				FROM uw06e02 p
				    INNER JOIN rf19 r
						ON r.kd_lok = p.kd_lok_asal
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_pol = @no_pol AND 
				      p.no_updt = @no_updt AND
				      p.no_rsk = @no_rsk AND
				      p.kd_endt = @kd_endt", 
                new { request.SearchKeyword, request.KodeCabang, 
                    request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_pol, request.no_updt, request.no_rsk,
                    request.kd_endt
                })).ToList();
        }
    }
}