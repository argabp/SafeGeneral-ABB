using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.JenisSors.Commands
{
    public class DeleteJenisSorCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_jns_sor { get; set; }
    }

    public class DeleteJenisSorCommandHandler : IRequestHandler<DeleteJenisSorCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteJenisSorCommandHandler> _logger;

        public DeleteJenisSorCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteJenisSorCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteJenisSorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var jenisSor = dbContext.JenisSor.FirstOrDefault(jenisSor => jenisSor.kd_jns_sor == request.kd_jns_sor);

                if (jenisSor != null)
                    dbContext.JenisSor.Remove(jenisSor);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}