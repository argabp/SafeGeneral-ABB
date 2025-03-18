using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using AutoMapper;

namespace ABB.Application.Users.Queries
{
    public class UserHistoryDto : IMapFrom<UserHistoryDto>, IMapFrom<AppUser>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public string Address { get; set; }
        public DateTime PasswordExpiredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserHistoryDto, UserHistory>();
            profile.CreateMap<AppUser, UserHistoryDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id));
        }
    }
}