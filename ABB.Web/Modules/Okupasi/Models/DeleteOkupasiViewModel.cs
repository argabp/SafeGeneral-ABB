using ABB.Application.Common.Interfaces;
using ABB.Application.Okupasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Okupasi.Models
{
    public class DeleteOkupasiViewModel : IMapFrom<DeleteOkupasiCommand>
    {
        public string kd_okup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteOkupasiViewModel, DeleteOkupasiCommand>();
        }
    }
}