using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class DeleteAkseptasiOtherMotorDetailCommand : IRequest
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

        public decimal thn_ptg_kend { get; set; }
    }

    public class DeleteAkseptasiOtherMotorDetailCommandHandler : IRequestHandler<DeleteAkseptasiOtherMotorDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteAkseptasiOtherMotorDetailCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteAkseptasiOtherMotorDetailCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.AkseptasiOtherMotorDetail.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt,
                request.no_rsk, request.kd_endt, request.thn_ptg_kend);

            if (entity == null)
                throw new NotFoundException();

            dbContext.AkseptasiOtherMotorDetail.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}