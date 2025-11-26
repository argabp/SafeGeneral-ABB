using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TypeCoas.Queries
{
    public class TypeCoaDto : IMapFrom<TypeCoa>
    {
        public string Nama { get; set; }
        public string Pos { get; set; }
        public string Dk { get; set; }
        public string Type { get; set; }

         public void Mapping(Profile profile)
        {
            profile.CreateMap<TypeCoa, TypeCoaDto>();
        }
    }
}