using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class DeletePengajuanAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
    }

    public class DeletePengajuanAkseptasiCommandHandler : IRequestHandler<DeletePengajuanAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeletePengajuanAkseptasiCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeletePengajuanAkseptasiCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.TRAkseptasi.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks);

            if (entity == null)
                throw new NotFoundException();
            
            var attachments = dbContext.TRAkseptasiAttachment.Where(w => w.kd_cb == request.kd_cb
                && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn 
                && w.no_aks == request.no_aks).ToList();
            
            dbContext.TRAkseptasiAttachment.RemoveRange(attachments);
            
            var akseptasiLimits = dbContext.AkseptasiLimit.Where(w => w.kd_cb == request.kd_cb
                                                                         && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn 
                                                                         && w.no_aks == request.no_aks).ToList();
            
            dbContext.AkseptasiLimit.RemoveRange(akseptasiLimits);
            
            dbContext.TRAkseptasi.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}