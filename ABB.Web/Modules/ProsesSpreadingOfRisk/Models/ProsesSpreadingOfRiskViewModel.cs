using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesSpreadingOfRisks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ProsesSpreadingOfRisk.Models
{
    public class ProsesSpreadingOfRiskViewModel : IMapFrom<AlokasiReasCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesSpreadingOfRiskViewModel, AlokasiReasCommand>();
        }
    }
}