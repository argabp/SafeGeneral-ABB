using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesAlokasiKlaimReasuransis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ProsesAlokasiKlaimReasuransi.Models
{
    public class ProsesAlokasiKlaimReasuransiViewModel : IMapFrom<AlokasiReasCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesAlokasiKlaimReasuransiViewModel, AlokasiReasCommand>();
        }
    }
}