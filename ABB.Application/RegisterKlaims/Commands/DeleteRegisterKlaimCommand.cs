using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RegisterKlaims.Commands
{
    public class DeleteRegisterKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
    }

    public class DeleteRegisterKlaimCommandHandler : IRequestHandler<DeleteRegisterKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteRegisterKlaimCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteRegisterKlaimCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.RegisterKlaim.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl);

            if (entity == null)
                throw new NotFoundException();
            
            var attachments = dbContext.DokumenRegisterKlaim.Where(w => w.kd_cb == request.kd_cb
                && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn 
                && w.no_kl == request.no_kl).ToList();
            
            dbContext.DokumenRegisterKlaim.RemoveRange(attachments);
            
            var analisaDanEvaluasis = dbContext.AnalisaDanEvaluasi.Where(w => w.kd_cb == request.kd_cb
                                                                          && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn 
                                                                          && w.no_kl == request.no_kl).ToList();
            
            dbContext.AnalisaDanEvaluasi.RemoveRange(analisaDanEvaluasis);
            
            var klaimStatusList = dbContext.KlaimStatus.Where(w => w.kd_cb == request.kd_cb
                                                                       && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn 
                                                                       && w.no_kl == request.no_kl).ToList();
            
            dbContext.KlaimStatus.RemoveRange(klaimStatusList);
            
            dbContext.RegisterKlaim.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}