using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Users.Commands
{
    public class CheckAuthorizationCommand : IRequest<bool>
    {
        public string UserID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

    public class CheckAuthorizationCommandHandler : IRequestHandler<CheckAuthorizationCommand, bool>
    {
        private readonly IDbConnection _db;

        public CheckAuthorizationCommandHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<bool> Handle(CheckAuthorizationCommand request, CancellationToken cancellationToken)
        {
            var result = await _db.QueryProc("sp_USR_CheckAuthorization", param: request);
            return result.ToList().Count > 0;
        }
    }
}