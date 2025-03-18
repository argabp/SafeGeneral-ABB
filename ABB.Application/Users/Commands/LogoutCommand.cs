using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using MediatR;

namespace ABB.Application.Users.Commands
{
    public class LogoutCommand : IRequest
    {
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly ISignInManagerHelper _signInManager;

        public LogoutCommandHandler(ISignInManagerHelper signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }
    }
}