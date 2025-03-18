using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class DeleteAkseptasiPranotaKoasCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }
    }

    public class DeleteAkseptasiPranotaKoasCommandHandler : IRequestHandler<DeleteAkseptasiPranotaKoasCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteAkseptasiPranotaKoasCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteAkseptasiPranotaKoasCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.AkseptasiPranotaKoas.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt,
                request.kd_mtu, request.kd_grp_pas, request.kd_rk_pas);

            if (entity == null)
                throw new NotFoundException();

            dbContext.AkseptasiPranotaKoas.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}