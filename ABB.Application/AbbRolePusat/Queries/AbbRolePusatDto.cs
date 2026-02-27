using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Application.Common.Dtos
{
    public class AbbRolePusatDto : IMapFrom<AbbRolePusat>
    {
        public int role_code { get; set; }
        public string role_nama { get; set; }

        public void Mapping(Profile profile)
        {
            // AutoMapper akan otomatis mapping Entity <-> DTO karena namanya sama persis
            profile.CreateMap<AbbRolePusat, AbbRolePusatDto>().ReverseMap();
        }
    }
}