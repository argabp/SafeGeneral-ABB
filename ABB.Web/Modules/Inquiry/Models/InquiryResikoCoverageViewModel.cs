using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryResikoCoverageViewModel : IMapFrom<InquiryCoverageDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string kd_cvrg { get; set; }

        public decimal pst_rate_prm { get; set; }

        public Int16 stn_rate_prm { get; set; }

        public decimal pst_dis { get; set; }

        public decimal pst_kms { get; set; }

        public string flag_pkk { get; set; }

        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryCoverageDto, InquiryResikoCoverageViewModel>();
        }
    }
}