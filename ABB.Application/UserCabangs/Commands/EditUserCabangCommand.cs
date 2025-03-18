using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.UserCabangs.Commands
{
    public class EditUserCabangCommand : IRequest
    {
        public EditUserCabangCommand()
        {
            Cabangs = new List<string>();
        }
        public string userid { get; set; }
        public List<string> Cabangs { get; set; }
    }

    public class EditUserCabangCommandHandler : IRequestHandler<EditUserCabangCommand>
    {
        private readonly IDbContext _context;

        public EditUserCabangCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditUserCabangCommand request, CancellationToken cancellationToken)
        {
            var userCabangs = _context.UserCabang.Where(w => w.userid == request.userid);
            
            _context.UserCabang.RemoveRange(userCabangs);
            
            for (var sequence = 0; sequence < request.Cabangs.Count; sequence++)
            {
                _context.UserCabang.Add(new UserCabang()
                {
                    userid = request.userid,
                    kd_cb = request.Cabangs[sequence]
                });
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}