using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SebabKejadians.Commands
{
    public class DeleteSebabKejadianCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_sebab { get; set; }
    }

    public class DeleteSebabKejadianCommandHandler : IRequestHandler<DeleteSebabKejadianCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteSebabKejadianCommandHandler> _logger;

        public DeleteSebabKejadianCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteSebabKejadianCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteSebabKejadianCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var sebabKejadian = dbContext.SebabKejadian.FirstOrDefault(sebabKejadian =>
                    sebabKejadian.kd_sebab == request.kd_sebab && sebabKejadian.kd_cob == request.kd_cob);

                if (sebabKejadian != null)
                    dbContext.SebabKejadian.Remove(sebabKejadian);

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