using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.PengajuanAkseptasi.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.PengajuanAkseptasi.Models
{
    public class ApprovalPengajuanAkseptasiModel : IMapFrom<ApprovalPengajuanAkseptasiCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }

        public List<IFormFile> Files { get; set; }

        public string nomor_pengajuan { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalPengajuanAkseptasiModel, ApprovalPengajuanAkseptasiCommand>();
        }
    }
}