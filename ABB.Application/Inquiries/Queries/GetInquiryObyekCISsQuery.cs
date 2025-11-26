using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryObyekCISsQuery : IRequest<List<InquiryObyekCISDto>>
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

    public class GetInquiryObyekCISsQueryHandler : IRequestHandler<GetInquiryObyekCISsQuery, List<InquiryObyekCISDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetInquiryObyekCISsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InquiryObyekCISDto>> Handle(GetInquiryObyekCISsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<InquiryObyekCISDto>(@"SELECT p.*
				FROM uw06e04 p
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