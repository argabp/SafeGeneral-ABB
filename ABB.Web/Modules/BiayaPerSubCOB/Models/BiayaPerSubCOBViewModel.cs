using ABB.Application.BiayaPerSubCOBs.Commands;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.BiayaPerSubCOB.Models
{
    public class BiayaPerSubCOBViewModel : IMapFrom<SaveBiayaPerSubCOBCommand>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_prm { get; set; }

        public decimal nilai_bia_pol { get; set; }

        public decimal nilai_bia_adm { get; set; }

        public decimal nilai_min_form { get; set; }

        public decimal nilai_maks_plafond { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BiayaPerSubCOBViewModel, SaveBiayaPerSubCOBCommand>();
            profile.CreateMap<BiayaPerSubCOBDto, BiayaPerSubCOBViewModel>();
        }
    }
}