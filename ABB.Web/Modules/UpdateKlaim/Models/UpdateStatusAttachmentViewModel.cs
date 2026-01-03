using System;
using ABB.Application.ApprovalMutasiKlaims.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.UpdateKlaim.Models
{
    public class UpdateStatusAttachmentViewModel : IMapFrom<GetPengajuanKlaimStatusAttachmentsQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
        
        public Int16 no_urut { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateStatusAttachmentViewModel, GetPengajuanKlaimStatusAttachmentsQuery>();
        }
    }
}