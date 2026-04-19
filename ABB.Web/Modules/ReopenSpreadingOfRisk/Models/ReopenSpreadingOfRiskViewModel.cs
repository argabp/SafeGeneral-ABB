using ABB.Application.Common.Interfaces;
using ABB.Application.ReopenSpreadingOfRisks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ReopenSpreadingOfRisk.Models
{
    public class ReopenSpreadingOfRiskViewModel : IMapFrom<ReopenSpreadingOfRiskCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_updt_reas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReopenSpreadingOfRiskViewModel, ReopenSpreadingOfRiskCommand>();
        }
    }
}