using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LevelOtoritass.Commands
{
    public class DeleteLevelOtoritasCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_user { get; set; }
    }

    public class DeleteLevelOtoritasCommandHandler : IRequestHandler<DeleteLevelOtoritasCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteLevelOtoritasCommandHandler> _logger;

        public DeleteLevelOtoritasCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteLevelOtoritasCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLevelOtoritasCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var levelOtoritas = dbContext.LevelOtoritas.FirstOrDefault(levelOtoritas => levelOtoritas.kd_user == request.kd_user);

                if (levelOtoritas != null)
                    dbContext.LevelOtoritas.Remove(levelOtoritas);

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