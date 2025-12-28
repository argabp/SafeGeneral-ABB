using ABB.Application.Common.Interfaces;
using ABB.Application.Rekanans.Commands;
using ABB.Application.TertanggungPrincipals.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Rekanan.Models
{
    public class DeleteRekananViewModel : IMapFrom<DeleteRekananCommand>
    {
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteRekananViewModel, DeleteRekananCommand>();
            profile.CreateMap<DeleteRekananViewModel, DeleteDetailRekananCommand>();
            profile.CreateMap<DeleteRekananViewModel, DeleteTertanggungPrincipalCommand>();
            profile.CreateMap<DeleteRekananViewModel, DeleteDetailTertanggungPrincipalCommand>();
            profile.CreateMap<DeleteRekananViewModel, DeleteDetailSlikCommand>();
        }
    }
}