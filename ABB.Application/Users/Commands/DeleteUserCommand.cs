using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Users.Queries;
using ABB.Domain.IdentityModels;
using AutoMapper;
using MediatR;

namespace ABB.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; } = true;
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUserManagerHelper userManager, IUserHistoryService userHistoryService
            , IMapper mapper)
        {
            _userManager = userManager;
            _userHistoryService = userHistoryService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            await AddUserHistory(user);

            return Unit.Value;
        }

        private async Task AddUserHistory(AppUser user)
        {
            var userhistory = _mapper.Map<UserHistoryDto>(user);
            _userHistoryService.AddUserHistory(userhistory);
            await _userHistoryService.Commit();
        }
    }
}