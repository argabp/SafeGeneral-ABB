using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetKeteranganQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }
        
        public Int16 no_updt { get; set; }
        
        public string no_rsk { get; set; }
    }

    public class GetKeteranganQueryHandler : IRequestHandler<GetKeteranganQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKeteranganQueryHandler> _logger;

        public GetKeteranganQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKeteranganQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetKeteranganQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_cl01e_08", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, 
                    request.kd_thn, request.no_pol, request.no_updt,
                    request.no_rsk
                })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}