using ABB.Application.BiayaMaterais.Commands;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.BiayaMaterai.Models
{
    public class BiayaMateraiViewModel : IMapFrom<SaveBiayaMateraiCommand>
    {
        public string kd_mtu { get; set; }

        public decimal nilai_prm_mul { get; set; }
        
        public decimal nilai_prm_akh { get; set; }

        public decimal nilai_bia_mat { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BiayaMateraiViewModel, SaveBiayaMateraiCommand>();
            profile.CreateMap<BiayaMateraiDto, BiayaMateraiViewModel>();
        }
    }
}