using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class CopyResikoViewModel : IMapFrom<CopyResikoCommand>
    {
        
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public string no_updt { get; set; }
        public string no_rsk { get; set; }
        public string kd_endt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyResikoViewModel, CopyResikoCommand>();
        }
    }
}