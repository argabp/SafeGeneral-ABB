using System;
using System.Collections.Generic;
using ABB.Application.ApprovalAkseptasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.ApprovalAkseptasi.Models
{
    public class ApprovalAkseptasiEscModel : IMapFrom<ApprovalAkseptasiEscCommand>
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

        public string kd_user_sign { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalAkseptasiEscModel, ApprovalAkseptasiEscCommand>();
            profile.CreateMap<ApprovalAkseptasiEscModel, ApprovalAkseptasiRevCommand>();
        }
    }
}