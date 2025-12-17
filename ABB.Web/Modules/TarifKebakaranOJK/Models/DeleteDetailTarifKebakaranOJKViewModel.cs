using ABB.Application.Common.Interfaces;
using ABB.Application.TarifKebakaranOJKs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TarifKebakaranOJK.Models
{
    public class DeleteDetailTarifKebakaranOJKViewModel : IMapFrom<DeleteDetailTarifKebakaranOJKCommand>
    {
        public string kd_okup { get; set; }
        
        public string kd_kls_konstr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailTarifKebakaranOJKViewModel, DeleteDetailTarifKebakaranOJKCommand>();
        }
    }
}