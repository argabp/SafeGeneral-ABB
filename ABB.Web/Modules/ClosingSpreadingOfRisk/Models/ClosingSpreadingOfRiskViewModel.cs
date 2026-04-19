using System;
using ABB.Application.ClosingSpreadingOfRisks.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.ClosingSpreadingOfRisk.Models
{
    public class ClosingSpreadingOfRiskViewModel : IMapFrom<ClosingSpreadingOfRiskCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public short no_updt_reas { get; set; }

        public int jk_bln { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClosingSpreadingOfRiskViewModel, ClosingSpreadingOfRiskCommand>();
        }
    }
}