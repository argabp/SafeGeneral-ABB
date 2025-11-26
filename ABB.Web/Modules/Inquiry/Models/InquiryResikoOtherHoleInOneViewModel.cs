using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryResikoOtherHoleInOneViewModel : IMapFrom<InquiryOtherHoleInOneDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string? turnamen { get; set; }

        public string? lokasi { get; set; }

        public string? jml_peserta { get; set; }

        public string? spek_hole { get; set; }

        public string? hadiah { get; set; }

        public string? own_risk { get; set; }

        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryOtherHoleInOneDto, InquiryResikoOtherHoleInOneViewModel>();
        }
    }
}