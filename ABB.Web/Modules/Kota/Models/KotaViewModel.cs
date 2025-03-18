using ABB.Application.Common.Interfaces;
using ABB.Application.Kotas.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Kota.Models
{
    public class KotaViewModel : IMapFrom<SaveKotaCommand>
    {
        public string kd_kota { get; set; }

        public string nm_kota { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KotaViewModel, SaveKotaCommand>();
            profile.CreateMap<KotaViewModel, DeleteKotaCommand>();
        }
    }
}