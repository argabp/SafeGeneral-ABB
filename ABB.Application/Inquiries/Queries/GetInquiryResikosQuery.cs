using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryResikosQuery : IRequest<List<InquiryResikoDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetInquiryResikosQueryHandler : IRequestHandler<GetInquiryResikosQuery, List<InquiryResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetInquiryResikosQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InquiryResikoDto>> Handle(GetInquiryResikosQuery request, CancellationToken cancellationToken)
        {
	        try
	        {
		        _connectionFactory.CreateDbConnection(request.DatabaseName);
		        var results = (await _connectionFactory.Query<InquiryResikoDto>(@"SELECT p.* 
				FROM uw04e p
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_pol = @no_pol AND 
				      p.no_updt = @no_updt", 
			        new { request.SearchKeyword, request.KodeCabang, 
				        request.kd_cob, request.kd_scob, request.kd_thn,
				        request.no_pol, request.no_updt
			        })).ToList();

		        var sequence = 0;
		        foreach (var result in results)
		        {
			        result.Id = sequence;
			        sequence++;
		        }

		        return results;
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine(e);
		        throw;
	        }
            
        }
    }
}