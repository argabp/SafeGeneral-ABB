using ABB.Application.Common.Interfaces;
using ABB.Application.MutasiKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MutasiKlaim.Models
{
    public class CopyAlokasiViewModel : MutasiKlaimModel, IMapFrom<CopyAlokasiCommand>
    {
        public decimal nilai_ttl_kl { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyAlokasiViewModel, CopyAlokasiCommand>();
        }
    }
}