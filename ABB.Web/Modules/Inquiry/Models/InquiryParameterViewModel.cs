using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryParameterViewModel : IMapFrom<GetInquiryQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryParameterViewModel, GetInquiryQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherFireQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherMotorQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherCargoQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherBondingQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherPAQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherHullQuery>();
            profile.CreateMap<InquiryParameterViewModel, GetInquiryOtherHoleInOneQuery>();
        }
    }
}