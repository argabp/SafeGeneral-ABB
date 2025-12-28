using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class EditDetailLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_lookup { get; set; }

        public int no_lookup { get; set; }

        public string nm_detail_lookup { get; set; }

        public string flag_lookup { get; set; }
    }

    public class EditDetailLookupCommandHandler : IRequestHandler<EditDetailLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDetailLookupCommandHandler> _logger;

        public EditDetailLookupCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDetailLookupCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDetailLookupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookupDetail = dbContext.LookupDetail.FirstOrDefault(w => w.kd_lookup == request.kd_lookup
                                                                            && w.no_lookup == request.no_lookup);

                if (lookupDetail != null)
                {
                    lookupDetail.nm_detail_lookup = request.nm_detail_lookup;
                    lookupDetail.flag_lookup = request.flag_lookup;
                }

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