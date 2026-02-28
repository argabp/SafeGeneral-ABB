using ABB.Application.Common.Interfaces;
using ABB.Application.PLAReasuransis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.PLAReasuransi.Models
{
    public class PLAReasuransiViewModel : IMapFrom<SavePLAReasuransiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_pla { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_pla { get; set; }

        public string flag_posting { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PLAReasuransiViewModel, SavePLAReasuransiCommand>();
            profile.CreateMap<Domain.Entities.PLAReasuransi, PLAReasuransiViewModel>();
        }
    }
}