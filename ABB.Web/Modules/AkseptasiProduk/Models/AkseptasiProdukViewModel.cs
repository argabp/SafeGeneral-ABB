using ABB.Application.AkseptasiProduks.Commands;
using ABB.Application.AkseptasiProduks.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.AkseptasiProduk.Models
{
    public class AkseptasiProdukViewModel : IMapFrom<AddAkseptasiProdukCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string Desc_AkseptasiProduk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiProdukViewModel, AddAkseptasiProdukCommand>();
            profile.CreateMap<AkseptasiProdukViewModel, EditAkseptasiProdukCommand>();
            profile.CreateMap<AkseptasiProdukDto, AkseptasiProdukViewModel>();
        }
    }
}