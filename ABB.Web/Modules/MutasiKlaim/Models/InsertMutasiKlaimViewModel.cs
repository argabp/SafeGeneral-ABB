using ABB.Application.Common.Interfaces;
using ABB.Application.MutasiKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MutasiKlaim.Models
{
    public class InsertMutasiKlaimViewModel : IMapFrom<InsertMutasiKlaimCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string tipe_mts { get; set; }

        public string kd_mtu { get; set; }

        public string flag_konv { get; set; }

        public string kd_usr_input { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InsertMutasiKlaimViewModel, InsertMutasiKlaimCommand>();
        }
    }
}