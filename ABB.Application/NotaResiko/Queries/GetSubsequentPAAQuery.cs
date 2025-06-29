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
    public class GetSubsequentPAAQuery : IRequest<List<SubsequentPAA>>
    {
        public Int64 Id { get; set; }
    }

    public class GetSubsequentPAAQueryHandler : IRequestHandler<GetSubsequentPAAQuery, List<SubsequentPAA>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetSubsequentPAAQueryHandler> _logger;

        public GetSubsequentPAAQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetSubsequentPAAQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<SubsequentPAA>> Handle(GetSubsequentPAAQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                return _dbContextCsm.SubsequentPAA.Where(w => w.Id == request.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}