using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;


namespace ABB.Application.TipeAkuns117.Queries
{
    public class TipeAkun117Dto : IMapFrom<TipeAkun117>
    {
        public string Kode { get; set; }
        public string NamaTipe { get; set; }
        public string Pos { get; set; }
        public bool? Kasbank { get; set; }

       public void Mapping(Profile profile)
        {
            profile.CreateMap<TipeAkun117, TipeAkun117Dto>();
        }
    }
}