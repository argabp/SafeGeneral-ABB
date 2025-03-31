using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NomorRegistrasiPolis.Queries
{
    public class GetNomorRegistrasiPolisQuery : IRequest<Domain.Entities.NomorRegistrasiPolis>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetNomorRegistrasiPolisQueryHandler : IRequestHandler<GetNomorRegistrasiPolisQuery, Domain.Entities.NomorRegistrasiPolis>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<GetNomorRegistrasiPolisQueryHandler> _logger;

        public GetNomorRegistrasiPolisQueryHandler(IDbContextFactory dbContextFactory, ILogger<GetNomorRegistrasiPolisQueryHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Domain.Entities.NomorRegistrasiPolis> Handle(GetNomorRegistrasiPolisQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);

                return dbContext.NomorRegistrasiPolis.Find(
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}