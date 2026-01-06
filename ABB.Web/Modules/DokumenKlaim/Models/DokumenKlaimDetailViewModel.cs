using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenKlaims.Commands;
using ABB.Application.DokumenKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DokumenKlaim.Models
{
    public class DokumenKlaimDetailViewModel : IMapFrom<SaveDokumenKlaimDetilCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public bool flag_wajib { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenKlaimDetailViewModel, SaveDokumenKlaimDetilCommand>();
            profile.CreateMap<DokumenKlaimDetilDto, DokumenKlaimDetailViewModel>();
        }
    }
}