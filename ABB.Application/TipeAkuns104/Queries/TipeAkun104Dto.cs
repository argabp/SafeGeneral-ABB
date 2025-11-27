using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;


namespace ABB.Application.TipeAkuns104.Queries
{
    public class TipeAkun104Dto : IMapFrom<TipeAkun104>
    {
        public string Kode { get; set; }
        public string NamaTipe { get; set; }
        public string Pos { get; set; }
        public bool? Kasbank { get; set; }

       public void Mapping(Profile profile)
        {
            profile.CreateMap<TipeAkun104, TipeAkun104Dto>();
        }
    }
}