using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Inquiries.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Inquiry.Models
{
    public class InquiryPranotaKoasViewModel : IMapFrom<InquiryPranotaKoasDto>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public decimal? pst_share { get; set; }

        public string? kd_prm { get; set; }

        public decimal? nilai_prm { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal? nilai_dis { get; set; }

        public decimal? pst_kms { get; set; }

        public decimal? nilai_kms { get; set; }

        public decimal? pst_hf { get; set; }

        public decimal? nilai_hf { get; set; }

        public decimal? nilai_kl { get; set; }

        public decimal? pst_pjk { get; set; }

        public decimal? nilai_pjk { get; set; }

        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InquiryPranotaKoasDto, InquiryPranotaKoasViewModel>();
        }
    }
}