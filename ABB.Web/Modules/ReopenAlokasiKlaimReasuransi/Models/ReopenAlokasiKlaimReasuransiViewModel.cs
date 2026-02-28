using ABB.Application.Common.Interfaces;
using ABB.Application.ReopenAlokasiKlaimReasuransis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ReopenAlokasiKlaimReasuransi.Models
{
    public class ReopenAlokasiKlaimReasuransiViewModel : IMapFrom<ReopenAlokasiKlaimReasuransiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReopenAlokasiKlaimReasuransiViewModel, ReopenAlokasiKlaimReasuransiCommand>();
        }
    }
}