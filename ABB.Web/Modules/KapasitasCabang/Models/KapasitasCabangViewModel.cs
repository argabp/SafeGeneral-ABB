using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.KapasitasCabangs.Commands;
using ABB.Application.KapasitasCabangs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KapasitasCabang.Models
{
    public class KapasitasCabangViewModel : IMapFrom<SaveKapasitasCabangCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public decimal nilai_kapasitas { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public decimal? nilai_kl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KapasitasCabangViewModel, SaveKapasitasCabangCommand>();
            profile.CreateMap<KapasitasCabangDto, KapasitasCabangViewModel>();
        }
    }
}