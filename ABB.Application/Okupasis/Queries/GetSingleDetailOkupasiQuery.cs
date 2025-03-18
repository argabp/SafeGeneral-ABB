using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Queries
{
    public class GetSingleDetailOkupasiQuery : IRequest<DetailOkupasi>
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class GetSingleDetailOkupasiQueryHandler : IRequestHandler<GetSingleDetailOkupasiQuery, DetailOkupasi>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetSingleDetailOkupasiQueryHandler> _logger;

        public GetSingleDetailOkupasiQueryHandler(IDbContextFactory contextFactory, ILogger<GetSingleDetailOkupasiQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<DetailOkupasi> Handle(GetSingleDetailOkupasiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.DetailOkupasi.FirstOrDefault(w =>
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