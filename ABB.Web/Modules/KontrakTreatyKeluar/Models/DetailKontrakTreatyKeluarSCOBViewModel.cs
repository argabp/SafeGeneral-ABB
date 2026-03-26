using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class DetailKontrakTreatyKeluarSCOBViewModel : IMapFrom<SaveDetailKontrakTreatyKeluarSCOBCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarSCOB, DetailKontrakTreatyKeluarSCOBViewModel>();
            profile.CreateMap<DetailKontrakTreatyKeluarSCOBViewModel, SaveDetailKontrakTreatyKeluarSCOBCommand>();
        }
    }
}