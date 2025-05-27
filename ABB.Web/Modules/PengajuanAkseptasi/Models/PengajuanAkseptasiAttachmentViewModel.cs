using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PengajuanAkseptasi.Commands;
using ABB.Application.PengajuanAkseptasi.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.PengajuanAkseptasi.Models
{
    public class PengajuanAkseptasiAttachmentViewModel : IMapFrom<PengajuanAkseptasiAttachmentDto>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 kd_jns_dokumen { get; set; }

        public Int16 kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }

        public IFormFile file { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PengajuanAkseptasiAttachmentViewModel, PengajuanAkseptasiAttachmentDto>();
            profile.CreateMap<PengajuanAkseptasiAttachmentViewModel, SavePengajuanAkspetasiAttachmentCommand>();
            profile.CreateMap<PengajuanAkseptasiAttachmentViewModel, DeletePengajuanAkseptasiAttachmentCommand>();
            profile.CreateMap<PengajuanAkseptasiAttachmentDto, PengajuanAkseptasiAttachmentViewModel>();
        }
    }
}