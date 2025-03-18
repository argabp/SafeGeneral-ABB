using ABB.Application.CetakSchedulePolis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakSchedulePolis.Models
{
    public class CetakSchedulePolisViewModel : IMapFrom<GetCetakSchedulePolisQuery>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string no_aks { get; set; }
        public int no_updt { get; set; }
        public string jenisLaporan { get; set; }
        public string bahasa { get; set; }
        public string jenisLampiran { get; set; }
        public int kd_thn { get; set; }
        public string nm_ttg { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakSchedulePolisViewModel, GetCetakSchedulePolisQuery>();
        }
    }
}