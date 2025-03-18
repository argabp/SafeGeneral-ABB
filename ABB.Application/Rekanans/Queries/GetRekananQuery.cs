using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Rekanans.Queries
{
    public class GetRekananQuery : IRequest<Rekanan>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GetRekananQueryHandler : IRequestHandler<GetRekananQuery, Rekanan>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetRekananQueryHandler> _logger;

        public GetRekananQueryHandler(IDbContextFactory contextFactory, ILogger<GetRekananQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Rekanan> Handle(GetRekananQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.Rekanan.FirstOrDefault(w =>
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