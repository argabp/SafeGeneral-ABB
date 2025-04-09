using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Application.Akseptasis.Queries
{
    public class CopyEndorsDto : IMapFrom<CopyEndorsInsertCommand>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string ket_rsk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyEndorsDto, CopyEndorsUpdateDeleteCommand>();
            profile.CreateMap<CopyEndorsDto, CopyEndorsInsertCommand>();
        }
    }
}