using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LimitKlaims.Commands;
using ABB.Application.LimitKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LimitKlaim.Models
{
    public class LimitKlaimViewModel : IMapFrom<AddLimitKlaimCommand>
    {
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitKlaimViewModel, AddLimitKlaimCommand>();
            profile.CreateMap<LimitKlaimViewModel, EditLimitKlaimCommand>();
            profile.CreateMap<LimitKlaimDto, LimitKlaimViewModel>();
        }
    }
}