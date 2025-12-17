using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKebakaranOJKs.Commands
{
    public class DeleteDetailTarifKebakaranOJKCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class DeleteDetailTarifKebakaranOJKCommandHandler : IRequestHandler<DeleteDetailTarifKebakaranOJKCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailTarifKebakaranOJKCommandHandler> _logger;

        public DeleteDetailTarifKebakaranOJKCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailTarifKebakaranOJKCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailTarifKebakaranOJKCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailTarifKebakaranOJK = dbContext.DetailTarifKebakaranOJK.FirstOrDefault(w => w.kd_okup == request.kd_okup
                    && w.kd_kls_konstr == request.kd_kls_konstr);

                if (detailTarifKebakaranOJK != null)
                    dbContext.DetailTarifKebakaranOJK.Remove(detailTarifKebakaranOJK);

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