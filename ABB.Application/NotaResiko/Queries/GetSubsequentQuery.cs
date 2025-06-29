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
    public class GetSubsequentQuery : IRequest<List<Subsequent>>
    {
        public Int64 Id { get; set; }
    }

    public class GetSubsequentQueryHandler : IRequestHandler<GetSubsequentQuery, List<Subsequent>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetSubsequentQueryHandler> _logger;

        public GetSubsequentQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetSubsequentQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<Subsequent>> Handle(GetSubsequentQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                return _dbContextCsm.Subsequent.Where(w => w.Id == request.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}