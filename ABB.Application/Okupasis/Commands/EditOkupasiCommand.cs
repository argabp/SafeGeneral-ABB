using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Commands
{
    public class EditOkupasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string nm_okup { get; set; }

        public string nm_okup_ing { get; set; }

        public string kd_category { get; set; }
    }

    public class EditOkupasiCommandHandler : IRequestHandler<EditOkupasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditOkupasiCommandHandler> _logger;

        public EditOkupasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditOkupasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditOkupasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var okupasi = dbContext.Okupasi.FirstOrDefault(w => w.kd_okup == request.kd_okup);

                if (okupasi != null)
                {
                    okupasi.nm_okup_ing = request.nm_okup_ing;
                    okupasi.nm_okup = request.nm_okup;
                    okupasi.kd_category = request.kd_category;
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