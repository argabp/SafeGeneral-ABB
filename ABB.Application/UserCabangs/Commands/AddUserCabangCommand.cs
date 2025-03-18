using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.UserCabangs.Commands
{
    public class AddUserCabangCommand : IRequest
    {
        public AddUserCabangCommand()
        {
            Cabangs = new List<string>();
        }
        public string userid { get; set; }
        public List<string> Cabangs { get; set; }
    }

    public class AddUserCabangCommandHandler : IRequestHandler<AddUserCabangCommand>
    {
        private readonly IDbContext _context;

        public AddUserCabangCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserCabangCommand request, CancellationToken cancellationToken)
        {
            for (var sequence = 0; sequence < request.Cabangs.Count; sequence++)
            {
                _context.UserCabang.Add(new UserCabang()
                {
                    userid = request.userid,
                    kd_cb = request.Cabangs[sequence],
                });
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}