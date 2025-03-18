using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.KodeKonfirmasis.Commands;
using ABB.Application.KodeKonfirmasis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KodeKonfirmasi.Models
{
    public class KodeKonfirmasiViewModel : IMapFrom<KodeKonfirmasiDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string kd_konfirm { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public string flag_polis { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<KodeKonfirmasiDto, KodeKonfirmasiViewModel>();
            profile.CreateMap<Domain.Entities.KodeKonfirmasi, KodeKonfirmasiViewModel>();
            profile.CreateMap<KodeKonfirmasiViewModel, AddKodeKonfirmasiCommand>();
        }
    }
}