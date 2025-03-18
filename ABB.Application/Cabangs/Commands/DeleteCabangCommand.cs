using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Cabangs.Commands
{
    public class DeleteCabangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
    }

    public class DeleteCabangCommandHandler : IRequestHandler<DeleteCabangCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteCabangCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteCabangCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.Cabang.FindAsync(request.kd_cb.Trim());
            if (entity == null)
                throw new NotFoundException();

            dbContext.Cabang.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}