using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class CopyObyekUnderwritingCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_rsk { get; set; }
    }

    public class CopyObyekUnderwritingCommandHandler : IRequestHandler<CopyObyekUnderwritingCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<CopyObyekUnderwritingCommandHandler> _logger;

        public CopyObyekUnderwritingCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<CopyObyekUnderwritingCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CopyObyekUnderwritingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc<InsertMutasiKlaimDto>("spe_cl02e_12", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts,
                    request.no_rsk
                });
                
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