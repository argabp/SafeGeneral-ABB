using ABB.Application.Alokasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Alokasi.Models
{
    public class EndorsAlokasiViewModel : IMapFrom<EndorsAlokasiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }
        
        public string ket_endt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EndorsAlokasiViewModel, EndorsAlokasiCommand>();
        }
    }
}