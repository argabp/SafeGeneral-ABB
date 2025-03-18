using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Queries
{
    public class GetSingleDetailLokasiResikoQuery : IRequest<DetailLokasiResiko>
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }

        public string kd_lok_rsk { get; set; }
    }

    public class GetSingleDetailLokasiResikoQueryHandler : IRequestHandler<GetSingleDetailLokasiResikoQuery, DetailLokasiResiko>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetSingleDetailLokasiResikoQueryHandler> _logger;

        public GetSingleDetailLokasiResikoQueryHandler(IDbContextFactory contextFactory, ILogger<GetSingleDetailLokasiResikoQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<DetailLokasiResiko> Handle(GetSingleDetailLokasiResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);

                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.DetailLokasiResiko.FirstOrDefault(w =>
                    w.kd_pos == request.kd_pos && w.kd_lok_rsk == request.kd_lok_rsk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}