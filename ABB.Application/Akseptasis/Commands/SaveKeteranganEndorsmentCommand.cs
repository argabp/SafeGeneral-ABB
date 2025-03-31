using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveKeteranganEndorsmentCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string no_endt { get; set; }

        public string? ket_endt { get; set; }
    }

    public class SaveKeteranganEndorsmentCommandHandler : IRequestHandler<SaveKeteranganEndorsmentCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveKeteranganEndorsmentCommandHandler> _logger;

        public SaveKeteranganEndorsmentCommandHandler(IDbContextFactory contextFactory,
            IDbConnectionFactory connectionFactory,
            ILogger<SaveKeteranganEndorsmentCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKeteranganEndorsmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.Akseptasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt);
            
                if (entity == null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    entity.no_endt = request.no_endt;
                    entity.ket_endt = request.ket_endt;

                    await dbContext.SaveChangesAsync(cancellationToken);
                }

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