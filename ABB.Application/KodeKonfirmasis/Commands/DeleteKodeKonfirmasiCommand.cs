using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.KodeKonfirmasis.Commands
{
    public class DeleteKodeKonfirmasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string kd_konfirm { get; set; }
    }

    public class DeleteKodeKonfirmasiCommandHandler : IRequestHandler<DeleteKodeKonfirmasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteKodeKonfirmasiCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteKodeKonfirmasiCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.KodeKonfirmasi.FindAsync(request.kd_cb.Trim(), request.kd_cob.Trim()
                , request.kd_scob.Trim(), request.kd_thn.Trim(), request.no_aks.Trim(), request.kd_konfirm.Trim());
            
            if (entity == null)
                throw new NotFoundException();

            dbContext.KodeKonfirmasi.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}