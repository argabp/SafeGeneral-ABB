using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryResikoOtherHullViewModel : IMapFrom<InquiryOtherHullDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string no_rangka { get; set; }

        public string no_msn { get; set; }

        public decimal? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryOtherHullDto, InquiryResikoOtherHullViewModel>();
        }
    }
}