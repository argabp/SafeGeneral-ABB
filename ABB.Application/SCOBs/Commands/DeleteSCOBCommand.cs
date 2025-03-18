using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.SCOBs.Commands
{
    public class DeleteSCOBCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
    }

    public class DeleteSCOBCommandHandler : IRequestHandler<DeleteSCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteSCOBCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteSCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.SCOB.FindAsync(request.kd_cob.Trim(), request.kd_scob.Trim());
            if (entity == null)
                throw new NotFoundException();

            dbContext.SCOB.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}