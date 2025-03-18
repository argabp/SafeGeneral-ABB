using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Users.Commands
{
    public class LoginCommand : IRequest<bool>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string UserDatabase { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly ISignInManagerHelper _signInManager;
        private readonly IDbContext _context;

        public LoginCommandHandler(IDbContext context, ISignInManagerHelper signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _context.User.FirstOrDefault(a => a.UserName == request.Username);
            var result = await _signInManager.PasswordSignInAsync(user, request.Password);
            return result;
        }
    }
}