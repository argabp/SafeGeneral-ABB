using ABB.Application.CetakNotaDanKwitansiPolis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakNotaDanKwitansiPolis.Models
{
    public class CetakNotaDanKwitansiPolisViewModel : IMapFrom<GetCetakNotaDanKwitansiPolisQuery>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string no_aks { get; set; }
        public string? no_pol_ttg { get; set; }
        public int no_updt { get; set; }
        public string jenisLaporan { get; set; }
        public string mataUang { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakNotaDanKwitansiPolisViewModel, GetCetakNotaDanKwitansiPolisQuery>();
        }
    }
}