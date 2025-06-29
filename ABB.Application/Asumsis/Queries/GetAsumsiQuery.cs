using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Queries
{
    public class GetAsumsiQuery : IRequest<List<Asumsi>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetAsumsiQueryHandler : IRequestHandler<GetAsumsiQuery, List<Asumsi>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetAsumsiQueryHandler> _logger;

        public GetAsumsiQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetAsumsiQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<Asumsi>> Handle(GetAsumsiQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            var result = new List<Asumsi>();
            try
            {
                result = _dbContextCsm.Asumsi.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return result;
        }
    }
}