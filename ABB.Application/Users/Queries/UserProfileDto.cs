using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using AutoMapper;

namespace ABB.Application.Users.Queries
{
    public class UserProfileDto : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, UserProfileDto>();
        }
    }
}