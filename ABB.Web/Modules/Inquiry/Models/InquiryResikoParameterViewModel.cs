using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryResikoParameterViewModel : IMapFrom<GetInquiryOtherFireQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public decimal? pst_share { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherFireQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherMotorQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherCargoQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherBondingQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherPAQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherHullQuery>();
            profile.CreateMap<InquiryResikoParameterViewModel, GetInquiryOtherHoleInOneQuery>();
        }
    }
}