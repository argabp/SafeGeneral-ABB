using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.EndorsementPolis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.EndorsementPolis.Models
{
    public class EndorsementPolisViewModel : IMapFrom<ProsesEndorsementNormalCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EndorsementPolisViewModel, ProsesEndorsementNormalCommand>();
            profile.CreateMap<EndorsementPolisViewModel, ProsesEndorsementCancelCommand>();
            profile.CreateMap<EndorsementPolisViewModel, ProsesEndorsementNoPremiumCommand>();
        }
    }
}