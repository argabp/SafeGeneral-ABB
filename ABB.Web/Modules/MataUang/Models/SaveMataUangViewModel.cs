using ABB.Application.Common.Interfaces;
using ABB.Application.MataUangs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MataUang.Models
{
    public class SaveMataUangViewModel : IMapFrom<AddMataUangCommand>
    {
        public string kd_mtu { get; set; }

        public string nm_mtu { get; set; }

        public string symbol { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveMataUangViewModel, AddMataUangCommand>();
            profile.CreateMap<SaveMataUangViewModel, EditMataUangCommand>();
        }
    }
}