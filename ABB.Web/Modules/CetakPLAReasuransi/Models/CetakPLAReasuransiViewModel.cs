using System;
using ABB.Application.CetakPLAReasuransis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakPLAReasuransi.Models
{
    public class CetakPLAReasuransiViewModel : IMapFrom<GetCetakPLAReasuransiQuery>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public Int16 no_pla { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakPLAReasuransiViewModel, GetCetakPLAReasuransiQuery>();
        }
    }
}