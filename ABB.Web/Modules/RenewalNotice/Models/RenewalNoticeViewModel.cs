using ABB.Application.Common.Interfaces;
using ABB.Application.RenewalNotices.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RenewalNotice.Models
{
    public class RenewalNoticeViewModel : IMapFrom<GetRenewalNoticeQuery>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_pol { get; set; }
        public string? nm_ttg { get; set; }
        public string? almt_ttg { get; set; }
        public int no_updt { get; set; }
        public string no_surat { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RenewalNoticeViewModel, GetRenewalNoticeQuery>();
        }
    }
}