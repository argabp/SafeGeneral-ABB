using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Queries
{
    public class GetKelasKonstruksiQuery : IRequest<List<KelasKonstruksi>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKelasKonstruksiQueryHandler : IRequestHandler<GetKelasKonstruksiQuery, List<KelasKonstruksi>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetKelasKonstruksiQueryHandler> _logger;

        public GetKelasKonstruksiQueryHandler(IDbContextFactory contextFactory, ILogger<GetKelasKonstruksiQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<List<KelasKonstruksi>> Handle(GetKelasKonstruksiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                return dbContext.KelasKonstruksi.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}