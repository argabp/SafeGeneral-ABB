using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.UserCabangs.Commands
{
    public class DeleteUserCabangCommand : IRequest
    {
        public string userid { get; set; }
    }

    public class DeleteUserCabangCommandHandler : IRequestHandler<DeleteUserCabangCommand>
    {
        private readonly IDbContext _context;

        public DeleteUserCabangCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCabangCommand request, CancellationToken cancellationToken)
        {
            var userCabangs = _context.UserCabang.Where(w => w.userid == request.userid);
            
            _context.UserCabang.RemoveRange(userCabangs);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}