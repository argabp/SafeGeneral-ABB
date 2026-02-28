using ABB.Application.Common.Interfaces;
using ABB.Application.DLAReasuransis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DLAReasuransi.Models
{
    public class DLAReasuransiViewModel : IMapFrom<SaveDLAReasuransiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_dla { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_dla { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DLAReasuransiViewModel, SaveDLAReasuransiCommand>();
            profile.CreateMap<Domain.Entities.DLAReasuransi, DLAReasuransiViewModel>();
        }
    }
}