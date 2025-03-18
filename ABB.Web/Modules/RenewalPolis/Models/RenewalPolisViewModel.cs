using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.RenewalPolis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.RenewalPolis.Models
{
    public class RenewalPolisViewModel : IMapFrom<RenewalPolisCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_scob_new { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RenewalPolisViewModel, RenewalPolisCommand>();
        }
    }
}