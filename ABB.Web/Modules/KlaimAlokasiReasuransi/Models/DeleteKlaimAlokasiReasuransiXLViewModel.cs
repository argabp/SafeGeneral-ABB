using ABB.Application.Common.Interfaces;
using ABB.Application.KlaimAlokasiReasuransis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.KlaimAlokasiReasuransi.Models
{
    public class DeleteKlaimAlokasiReasuransiXLViewModel : IMapFrom<DeleteKlaimAlokasiReasuransiXLCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_kontr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteKlaimAlokasiReasuransiXLViewModel, DeleteKlaimAlokasiReasuransiXLCommand>();
        }
    }
}