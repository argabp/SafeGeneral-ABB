using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class AddDetailLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_lookup { get; set; }

        public int no_lookup { get; set; }

        public string nm_detail_lookup { get; set; }

        public string flag_lookup { get; set; }
    }

    public class AddDetailLookupCommandHandler : IRequestHandler<AddDetailLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDetailLookupCommandHandler> _logger;

        public AddDetailLookupCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDetailLookupCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDetailLookupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookupDetail = new LookupDetail()
                {
                    kd_lookup = request.kd_lookup,
                    no_lookup = request.no_lookup,
                    nm_detail_lookup = request.nm_detail_lookup,
                    flag_lookup = request.flag_lookup
                };

                dbContext.LookupDetail.Add(lookupDetail);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}