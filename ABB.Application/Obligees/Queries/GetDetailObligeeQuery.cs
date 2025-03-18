using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Obligees.Queries
{
    public class GetDetailObligeeQuery : IRequest<DetailObligee>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GetDetailObligeeQueryHandler : IRequestHandler<GetDetailObligeeQuery, DetailObligee>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetDetailObligeeQueryHandler> _logger;

        public GetDetailObligeeQueryHandler(IDbContextFactory contextFactory, ILogger<GetDetailObligeeQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<DetailObligee> Handle(GetDetailObligeeQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.DetailObligee.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb.Trim() && w.kd_grp_rk == request.kd_grp_rk.Trim() && w.kd_rk == request.kd_rk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}