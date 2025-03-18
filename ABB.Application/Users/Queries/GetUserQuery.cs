using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ABB.Application.Users.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IDbConnection _db;
        private readonly IConfiguration _config;

        public GetUserQueryHandler(IDbConnection db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _db.QueryFirstProc<UserDto>("sp_USR_GetUser", new { request.UserId });
            var urlPhoto = $"{_config.GetSection("ProfilePictureFolder").Value}/{user.Photo}";
            if (user?.Photo.Contains("default") ?? false)
                user.Photo = "/img/circular-upload.png";
            else
                user.Photo = urlPhoto;
            return user;
        }
    }
}