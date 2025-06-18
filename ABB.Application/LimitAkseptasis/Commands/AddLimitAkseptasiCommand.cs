using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class AddLimitAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_limit { get; set; }
    }

    public class AddLimitAkseptasiCommandHandler : IRequestHandler<AddLimitAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddLimitAkseptasiCommandHandler> _logger;

        public AddLimitAkseptasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddLimitAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasi = new LimitAkseptasi()
                {
                    kd_cb = request.kd_cb,
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    nm_limit = request.nm_limit,
                };

                dbContext.LimitAkseptasi.Add(limitAkseptasi);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}