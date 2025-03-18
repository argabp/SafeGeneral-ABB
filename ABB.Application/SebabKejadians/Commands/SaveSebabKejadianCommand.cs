using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SebabKejadians.Commands
{
    public class SaveSebabKejadianCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_sebab { get; set; }

        public string nm_sebab { get; set; }
    }

    public class SaveSebabKejadianCommandHandler : IRequestHandler<SaveSebabKejadianCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveSebabKejadianCommandHandler> _logger;

        public SaveSebabKejadianCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveSebabKejadianCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveSebabKejadianCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var sebabKejadian = dbContext.SebabKejadian.FirstOrDefault(sebabKejadian =>
                    sebabKejadian.kd_cob == request.kd_cob && request.kd_sebab == sebabKejadian.kd_sebab);

                if (sebabKejadian == null)
                {
                    dbContext.SebabKejadian.Add(new SebabKejadian()
                    {
                        kd_cob = request.kd_cob,
                        kd_sebab = request.kd_sebab,
                        nm_sebab = request.nm_sebab
                    });
                }
                else
                    sebabKejadian.nm_sebab = request.nm_sebab;

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