using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.OutstandingKlaim.Queries;
using AutoMapper;

namespace ABB.Web.Modules.OutstandingKlaim.Models
{
    public class OutstandingKlaimViewModel : IMapFrom<GetOutstandingKlaimQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OutstandingKlaimViewModel, GetOutstandingKlaimQuery>();
        }
    }
}