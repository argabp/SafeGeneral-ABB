using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class AddLimitAkseptasiDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }

        public decimal nilai_limit_akhir { get; set; }
    }

    public class AddLimitAkseptasiDetilCommandHandler : IRequestHandler<AddLimitAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddLimitAkseptasiDetilCommandHandler> _logger;

        public AddLimitAkseptasiDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddLimitAkseptasiDetilCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitAkseptasiDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasiDetil = new LimitAkseptasiDetil()
                {
                    kd_cb = request.kd_cb,
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    kd_user = request.kd_user,
                    nilai_limit_awal = request.nilai_limit_awal,
                    nilai_limit_akhir = request.nilai_limit_akhir
                };

                dbContext.LimitAkseptasiDetil.Add(limitAkseptasiDetil);

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