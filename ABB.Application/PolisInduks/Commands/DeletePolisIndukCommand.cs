using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PolisInduks.Commands
{
    public class DeletePolisIndukCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string no_pol_induk { get; set; }
    }

    public class DeletePolisIndukCommandHandler : IRequestHandler<DeletePolisIndukCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeletePolisIndukCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeletePolisIndukCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.PolisInduk.FindAsync(request.no_pol_induk.Trim());
            
            if (entity == null)
                throw new NotFoundException();

            dbContext.PolisInduk.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}