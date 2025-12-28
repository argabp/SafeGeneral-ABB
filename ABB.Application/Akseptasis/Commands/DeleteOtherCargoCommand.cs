using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class DeleteOtherCargoCommand : IRequest
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
    }

    public class DeleteOtherCargoCommandHandler : IRequestHandler<DeleteOtherCargoCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteOtherCargoCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteOtherCargoCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            
            var akseptasiResiko = await dbContext.AkseptasiOtherCargo.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                request.no_rsk, request.kd_endt);

            if (akseptasiResiko != null)
            {
                var details = dbContext.AkseptasiOtherCargoDetail.Where(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn &&
                    w.no_aks == request.no_aks &&
                    w.no_updt == request.no_updt &&
                    w.no_rsk == request.no_rsk &&
                    w.kd_endt == request.kd_endt).ToList();
                
                dbContext.AkseptasiOtherCargoDetail.RemoveRange(details);
                
                dbContext.AkseptasiOtherCargo.Remove(akseptasiResiko);
                
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }
    }
}