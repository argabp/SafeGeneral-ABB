using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKebakaranOJKs.Queries
{
    public class GetDetailTarifKebakaranOJKQuery : IRequest<DetailTarifKebakaranOJK>
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class GetDetailTarifKebakaranOJKQueryHandler : IRequestHandler<GetDetailTarifKebakaranOJKQuery, DetailTarifKebakaranOJK>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetDetailTarifKebakaranOJKQueryHandler> _logger;

        public GetDetailTarifKebakaranOJKQueryHandler(IDbContextFactory contextFactory, ILogger<GetDetailTarifKebakaranOJKQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<DetailTarifKebakaranOJK> Handle(GetDetailTarifKebakaranOJKQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.DetailTarifKebakaranOJK.FirstOrDefault(w =>
                    w.kd_okup == request.kd_okup.Trim() && w.kd_kls_konstr == request.kd_kls_konstr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}