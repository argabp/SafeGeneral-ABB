using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.COBs.Commands
{
    public class DeleteCOBCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
    }

    public class DeleteCOBCommandHandler : IRequestHandler<DeleteCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteCOBCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.COB.FindAsync(request.kd_cob.Trim());
            if (entity == null)
                throw new NotFoundException();

            dbContext.COB.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}