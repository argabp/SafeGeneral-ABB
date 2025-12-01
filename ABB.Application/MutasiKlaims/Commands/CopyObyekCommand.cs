using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class CopyObyekCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
    }

    public class CopyObyekCommandHandler : IRequestHandler<CopyObyekCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<CopyObyekCommandHandler> _logger;

        public CopyObyekCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<CopyObyekCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CopyObyekCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var result = (await _connectionFactory.QueryProc<string>("spe_cl02e_07", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts
                })).FirstOrDefault();
                
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}