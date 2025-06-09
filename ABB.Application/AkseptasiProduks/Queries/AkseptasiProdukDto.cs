using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.AkseptasiProduks.Queries
{
    public class AkseptasiProdukDto : IMapFrom<AkseptasiProduk>
    {
        public int Id { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        
        public string Desc_AkseptasiProduk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiProduk, AkseptasiProdukDto>();
        }
    }
}