using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenAkseptasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DokumenAkseptasi.Models
{
    public class DeleteDokumenAkseptasiDetailViewModel : IMapFrom<DeleteDokumenAkseptasiDetilCommand>
    {

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDokumenAkseptasiDetailViewModel, DeleteDokumenAkseptasiDetilCommand>();
        }
    }
}