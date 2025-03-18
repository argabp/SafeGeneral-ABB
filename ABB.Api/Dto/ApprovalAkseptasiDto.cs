using System;
using System.Collections.Generic;
using ABB.Application.Approval.Commands;
using ABB.Application.Common.Interfaces;
using ABB.Application.PengajuanAkseptasi.Commands;
using AutoMapper;

namespace ABB.Api.Dto
{
    public class ApprovalAkseptasiDto : IMapFrom<ApprovalAkseptasiNasLifeCommand>
    {
        public string authkey { get; set; }

        public string no_sppa { get; set; }

        public DateTime tgl_approval { get; set; }

        public string status { get; set; }

        public string kategori { get; set; }

        public string rate { get; set; }

        public string premi { get; set; }

        public string extra_premi { get; set; }

        public Int16? st_confirm { get; set; }

        public string rate_org { get; set; }

        public string file_cover_note { get; set; }

        public string remark { get; set; }

        public List<DataAkseptasiNasLifeDokumenDto> dokumen { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalAkseptasiDto, ApprovalAkseptasiNasLifeCommand>();
            profile.CreateMap<ApprovalAkseptasiDto, ConfirmNasLifeCommand>();
        }
    }
}