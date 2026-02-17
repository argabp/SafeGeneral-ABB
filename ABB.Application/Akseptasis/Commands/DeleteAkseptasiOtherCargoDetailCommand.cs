using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class DeleteAkseptasiOtherCargoDetailCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_urut { get; set; }
    }

    public class DeleteAkseptasiOtherCargoDetailCommandHandler : IRequestHandler<DeleteAkseptasiOtherCargoDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        private readonly ILogger<DeleteAkseptasiOtherCargoDetailCommandHandler> _logger;

        public DeleteAkseptasiOtherCargoDetailCommandHandler(IDbContextFactory contextFactory,
            ILogger<DeleteAkseptasiOtherCargoDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAkseptasiOtherCargoDetailCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var entity = await dbContext.AkseptasiOtherCargoDetail.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt,
                    request.no_rsk, request.kd_endt, request.no_urut);

                if (entity == null)
                    throw new NotFoundException();

                dbContext.AkseptasiOtherCargoDetail.Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }, _logger);
        }
    }
}