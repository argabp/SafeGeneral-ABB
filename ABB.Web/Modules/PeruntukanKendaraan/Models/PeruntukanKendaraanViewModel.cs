using ABB.Application.Common.Interfaces;
using ABB.Application.PeruntukanKendaraans.Commands;
using AutoMapper;

namespace ABB.Web.Modules.PeruntukanKendaraan.Models
{
    public class PeruntukanKendaraanViewModel : IMapFrom<SavePeruntukanKendaraanCommand>
    {
        public string kd_utk { get; set; }

        public string nm_utk { get; set; }

        public string nm_utk_ing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PeruntukanKendaraanViewModel, SavePeruntukanKendaraanCommand>();
            profile.CreateMap<PeruntukanKendaraanViewModel, DeletePeruntukanKendaraanCommand>();
        }
    }
}