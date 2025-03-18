using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Kotas.Commands;
using ABB.Application.PertanggunganKendaraans.Commands;
using ABB.Application.PertanggunganKendaraans.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PertanggunganKendaraan.Models
{
    public class PertanggunganKendaraanViewModel : IMapFrom<SaveKotaCommand>
    {
        public string kd_cob { get; set; }

        public string? kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public string? desk { get; set; }

        public Int16? jml_hari { get; set; }

        public string? ket_klasula { get; set; }

        public string? flag_tjh { get; set; }

        public string? flag_rscc { get; set; }

        public string? flag_banjir { get; set; }

        public string? flag_accessories { get; set; }

        public string? flag_lain_lain01 { get; set; }
        
        public string? flag_lain_lain02 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PertanggunganKendaraanViewModel, AddPertanggunganKendaraanCommand>();
            profile.CreateMap<PertanggunganKendaraanViewModel, DeletePertanggunganKendaraanCommand>();
            profile.CreateMap<PertanggunganKendaraanViewModel, EditPertanggunganKendaraanCommand>();
            profile.CreateMap<PertanggunganKendaraanDto, PertanggunganKendaraanViewModel>();
        }
    }
}