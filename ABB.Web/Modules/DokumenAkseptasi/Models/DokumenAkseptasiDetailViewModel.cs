using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenAkseptasis.Commands;
using ABB.Application.DokumenAkseptasis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DokumenAkseptasi.Models
{
    public class DokumenAkseptasiDetailViewModel : IMapFrom<AddDokumenAkseptasiDetilCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public bool flag_wajib { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenAkseptasiDetailViewModel, AddDokumenAkseptasiDetilCommand>();
            profile.CreateMap<DokumenAkseptasiDetailViewModel, EditDokumenAkseptasiDetilCommand>();
            profile.CreateMap<DokumenAkseptasiDetilDto, DokumenAkseptasiDetailViewModel>();
        }
    }
}