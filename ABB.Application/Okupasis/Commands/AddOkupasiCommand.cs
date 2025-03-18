using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Commands
{
    public class AddOkupasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string nm_okup { get; set; }

        public string nm_okup_ing { get; set; }

        public string kd_category { get; set; }
    }

    public class AddOkupasiCommandHandler : IRequestHandler<AddOkupasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddOkupasiCommandHandler> _logger;

        public AddOkupasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddOkupasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddOkupasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var okupasi = new Okupasi()
                {
                    kd_category = request.kd_category,
                    kd_okup = request.kd_okup,
                    nm_okup = request.nm_okup,
                    nm_okup_ing = request.nm_okup_ing
                };

                dbContext.Okupasi.Add(okupasi);

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