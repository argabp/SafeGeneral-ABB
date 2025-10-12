using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryResikoOtherCargoDetailViewModel : IMapFrom<InquiryOtherCargoDetailDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_urut { get; set; }

        public string jns_angkut { get; set; }

        public string? kd_angkut { get; set; }

        public string nm_angkut { get; set; }

        public string no_bl { get; set; }
        
        public string no_inv { get; set; }
        
        public string no_po { get; set; }
        
        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryOtherCargoDetailDto, InquiryResikoOtherCargoDetailViewModel>();
        }
    }
}