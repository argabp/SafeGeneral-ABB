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
    public class GetCopyEndorsQuery : IRequest<List<CopyEndorsDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetCopyEndorsQueryHandler : IRequestHandler<GetCopyEndorsQuery, List<CopyEndorsDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCopyEndorsQueryHandler> _logger;
        public GetCopyEndorsQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GetCopyEndorsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CopyEndorsDto>> Handle(GetCopyEndorsQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var datas = (await _connectionFactory.Query<CopyEndorsDto>(@"SELECT * FROM uw04c WHERE kd_cb = @kd_cb AND 
                            kd_cob = @kd_cob AND kd_scob = @kd_scob AND kd_thn = @kd_thn AND 
                            no_pol = @no_pol", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, 
                    request.kd_thn, request.no_pol
                })).ToList();

                int sequence = 0;
                foreach (var data in datas)
                {
                    sequence++;
                    data.Id = sequence;
                }

                return datas;
            }, _logger);
        }
    }
}