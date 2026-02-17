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
	public class GetAkseptasiObyeksQuery : IRequest<List<AkseptasiObyekDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }
        
        public string kd_endt { get; set; }
    }

    public class GetAkseptasiObyeksQueryHandler : IRequestHandler<GetAkseptasiObyeksQuery, List<AkseptasiObyekDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkseptasiObyeksQueryHandler> _logger;

        public GetAkseptasiObyeksQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GetAkseptasiObyeksQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<AkseptasiObyekDto>> Handle(GetAkseptasiObyeksQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<AkseptasiObyekDto>(@"SELECT p.*
				FROM uw06a01 p
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_aks = @no_aks AND 
				      p.no_updt = @no_updt AND
				      p.no_rsk = @no_rsk AND
				      p.kd_endt = @kd_endt", 
	            new { request.SearchKeyword, request.KodeCabang, 
		            request.kd_cob, request.kd_scob, request.kd_thn,
		            request.no_aks, request.no_updt, request.no_rsk,
		            request.kd_endt
	            })).ToList();
            }, _logger);
        }
    }
}