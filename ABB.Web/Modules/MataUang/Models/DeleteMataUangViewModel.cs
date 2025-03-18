using ABB.Application.Common.Interfaces;
using ABB.Application.MataUangs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MataUang.Models
{
    public class DeleteMataUangViewModel : IMapFrom<DeleteMataUangCommand>
    {
        public string kd_mtu { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteMataUangViewModel, DeleteMataUangCommand>();
        }
    }
}