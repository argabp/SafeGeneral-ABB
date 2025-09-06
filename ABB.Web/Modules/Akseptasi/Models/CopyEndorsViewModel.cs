using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class CopyEndorsViewModel : IMapFrom<CopyEndorsInsertCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string flag_endt { get; set; }
        

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyEndorsViewModel, CopyEndorsUpdateDeleteCommand>();
            profile.CreateMap<CopyEndorsViewModel, CopyEndorsInsertCommand>();
        }
    }
}