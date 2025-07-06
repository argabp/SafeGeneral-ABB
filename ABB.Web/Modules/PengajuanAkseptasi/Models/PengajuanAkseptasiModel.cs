using ABB.Application.Common.Interfaces;
using ABB.Application.PengajuanAkseptasi.Commands;
using ABB.Application.PengajuanAkseptasi.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PengajuanAkseptasi.Models
{
    public class PengajuanAkseptasiModel : IMapFrom<GetPengajuanAkseptasiStatusQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PengajuanAkseptasiModel, GetPengajuanAkseptasiStatusQuery>();
            profile.CreateMap<PengajuanAkseptasiModel, GetPengajuanAkseptasiAttachmentQuery>();
            profile.CreateMap<PengajuanAkseptasiModel, GetPengajuanAkseptasiQuery>();
            profile.CreateMap<PengajuanAkseptasiModel, GetReportPengajuanAkseptasiQuery>();
            profile.CreateMap<PengajuanAkseptasiModel, GetReportKeteranganPengajuanAkseptasiQuery>();
        }
    }
}