using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LevelOtoritass.Commands
{
    public class SaveLevelOtoritasCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_user { get; set; }

        public string kd_pass { get; set; }

        public string? flag_xol { get; set; }

        public decimal? nilai_xol { get; set; }
    }

    public class SaveLevelOtoritasCommandHandler : IRequestHandler<SaveLevelOtoritasCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveLevelOtoritasCommandHandler> _logger;

        public SaveLevelOtoritasCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveLevelOtoritasCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveLevelOtoritasCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var levelOtoritas = dbContext.LevelOtoritas.FirstOrDefault(w => w.kd_user == request.kd_user);

                if (levelOtoritas == null)
                {
                    dbContext.LevelOtoritas.Add(new Domain.Entities.LevelOtoritas()
                    {
                        kd_user = request.kd_user,
                        kd_pass = request.kd_pass,
                        flag_xol = request.flag_xol,
                        nilai_xol = request.nilai_xol
                    });
                }
                else
                {
                    levelOtoritas.kd_pass = request.kd_pass;
                    levelOtoritas.flag_xol = request.flag_xol;
                    levelOtoritas.nilai_xol = request.nilai_xol;
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