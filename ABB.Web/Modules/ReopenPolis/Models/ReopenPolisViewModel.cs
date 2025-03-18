using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ReopenPolis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ReopenPolis.Models
{
    public class ReopenPolisViewModel : IMapFrom<ReopenPolisCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReopenPolisViewModel, ReopenPolisCommand>();
        }
    }
}