using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DokumenKlaim.Models
{
    public class DeleteDokumenKlaimDetailViewModel : IMapFrom<DeleteDokumenKlaimDetilCommand>
    {

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDokumenKlaimDetailViewModel, DeleteDokumenKlaimDetilCommand>();
        }
    }
}