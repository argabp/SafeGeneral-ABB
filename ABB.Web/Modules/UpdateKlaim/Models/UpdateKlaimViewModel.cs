using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.UpdateKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.UpdateKlaim.Models
{
    public class UpdateKlaimViewModel : IMapFrom<UpdateKlaimCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }

        public string nomor_berkas { get; set; }

        public string status_name { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateKlaimViewModel, UpdateKlaimCommand>();
        }
    }
}