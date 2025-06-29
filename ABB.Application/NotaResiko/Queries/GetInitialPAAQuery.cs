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
    public class GetInitialPAAQuery : IRequest<List<InitialPAA>>
    {
        public Int64 Id { get; set; }
    }

    public class GetInitialPAAQueryHandler : IRequestHandler<GetInitialPAAQuery, List<InitialPAA>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetInitialPAAQueryHandler> _logger;

        public GetInitialPAAQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetInitialPAAQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<InitialPAA>> Handle(GetInitialPAAQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                return _dbContextCsm.InitialPAA.Where(w => w.Id == request.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}