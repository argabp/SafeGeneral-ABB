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
    public class GetIntialReleaseQuery : IRequest<List<IntialRelease>>
    {
        public Int64 Id { get; set; }
    }

    public class GetIntialReleaseQueryHandler : IRequestHandler<GetIntialReleaseQuery, List<IntialRelease>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<GetIntialReleaseQueryHandler> _logger;

        public GetIntialReleaseQueryHandler(IDbContextCSM dbContextCsm, ILogger<GetIntialReleaseQueryHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<List<IntialRelease>> Handle(GetIntialReleaseQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                return _dbContextCsm.IntialRelease.Where(w => w.Id == request.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}