using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiResikosQuery : IRequest<List<AkseptasiResikoDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetAkseptasiResikosQueryHandler : IRequestHandler<GetAkseptasiResikosQuery, List<AkseptasiResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
		private readonly ILogger<GetAkseptasiResikosQueryHandler> _logger;

        public GetAkseptasiResikosQueryHandler(IDbConnectionFactory connectionFactory,
			ILogger<GetAkseptasiResikosQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<AkseptasiResikoDto>> Handle(GetAkseptasiResikosQuery request, CancellationToken cancellationToken)
        {
	        return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
	        {
		        _connectionFactory.CreateDbConnection(request.DatabaseName);
		        var results = (await _connectionFactory.Query<AkseptasiResikoDto>(@"SELECT p.* 
				FROM uw04a p
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_aks = @no_aks AND 
				      p.no_updt = @no_updt", 
			        new { request.SearchKeyword, request.KodeCabang, 
				        request.kd_cob, request.kd_scob, request.kd_thn,
				        request.no_aks, request.no_updt
			        })).ToList();

		        var sequence = 0;
		        foreach (var result in results)
		        {
			        result.Id = sequence;
			        sequence++;
		        }

		        return results;
			}, _logger);
            
        }
    }
}