using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaResiko.Queries
{
    public class GetIntialLiabilityQuery : IRequest<List<IntialLiability>>
    {
        public Int64 Id { get; set; }
    }

    public class GetIntialLiabilityQueryHandler : IRequestHandler<GetIntialLiabilityQuery, List<IntialLiability>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetIntialLiabilityQueryHandler> _logger;

        public GetIntialLiabilityQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetIntialLiabilityQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<IntialLiability>> Handle(GetIntialLiabilityQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                return _dbContextCsm.IntialLiability.Where(w => w.Id == request.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}