using System;
using System.Collections.Generic;
using ABB.Application.ApprovalMutasiKlaims.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.ApprovalMutasiKlaim.Models
{
    public class ApprovalMutasiKlaimModel : IMapFrom<ApprovalMutasiKlaimCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }

        public List<IFormFile> Files { get; set; }

        public string nomor_berkas { get; set; }

        public string status_name { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalMutasiKlaimModel, ApprovalMutasiKlaimCommand>();
            profile.CreateMap<ApprovalMutasiKlaimModel, ApprovalMutasiKlaimClosedCommand>();
        }
    }
}