using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Commands
{
    public class DeleteDetailOkupasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class DeleteDetailOkupasiCommandHandler : IRequestHandler<DeleteDetailOkupasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailOkupasiCommandHandler> _logger;

        public DeleteDetailOkupasiCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailOkupasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailOkupasiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailOkupasi = dbContext.DetailOkupasi.FirstOrDefault(w => w.kd_okup == request.kd_okup
                    && w.kd_kls_konstr == request.kd_kls_konstr);

                if (detailOkupasi != null)
                    dbContext.DetailOkupasi.Remove(detailOkupasi);

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