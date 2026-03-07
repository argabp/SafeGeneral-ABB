using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.OutstandingKlaimReasuransis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.OutstandingKlaimReasuransi.Models
{
    public class OutstandingKlaimReasuransiViewModel : IMapFrom<GetOutstandingKlaimReasuransiQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OutstandingKlaimReasuransiViewModel, GetOutstandingKlaimReasuransiQuery>();
        }
    }
}