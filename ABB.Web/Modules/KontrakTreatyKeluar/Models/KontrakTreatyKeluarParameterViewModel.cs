using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Application.KontrakTreatyKeluars.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class KontrakTreatyKeluarParameterViewModel : IMapFrom<GetKontrakTreatyKeluarQuery>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KontrakTreatyKeluarParameterViewModel, GetKontrakTreatyKeluarQuery>();
        }
    }
}