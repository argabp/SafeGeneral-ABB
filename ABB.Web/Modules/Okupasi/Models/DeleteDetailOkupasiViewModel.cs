using ABB.Application.Common.Interfaces;
using ABB.Application.Okupasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Okupasi.Models
{
    public class DeleteDetailOkupasiViewModel : DeleteOkupasiViewModel, IMapFrom<DeleteDetailOkupasiCommand>
    {
        public string kd_kls_konstr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailOkupasiViewModel, DeleteDetailOkupasiCommand>();
        }
    }
}