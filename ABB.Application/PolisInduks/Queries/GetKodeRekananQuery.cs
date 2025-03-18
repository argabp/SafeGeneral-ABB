using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetKodeRekananQuery : IRequest<List<Rekanan>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeRekananQueryHandler : IRequestHandler<GetKodeRekananQuery, List<Rekanan>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetKodeRekananQueryHandler> _logger;

        public GetKodeRekananQueryHandler(IDbContextFactory contextFactory, ILogger<GetKodeRekananQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<List<Rekanan>> Handle(GetKodeRekananQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                return dbContext.Rekanan.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}