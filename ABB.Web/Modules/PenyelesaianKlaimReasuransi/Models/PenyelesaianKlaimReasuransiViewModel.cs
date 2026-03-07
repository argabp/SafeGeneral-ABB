using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PenyelesaianKlaimReasuransis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PenyelesaianKlaimReasuransi.Models
{
    public class PenyelesaianKlaimReasuransiViewModel : IMapFrom<GetPenyelesaianKlaimReasuransiQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PenyelesaianKlaimReasuransiViewModel, GetPenyelesaianKlaimReasuransiQuery>();
        }
    }
}