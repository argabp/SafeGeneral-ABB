using ABB.Application.CetakSlipKomisiFakultatifKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakSlipKomisiFakultatifKeluar.Models
{
    public class CetakSlipKomisiFakultatifKeluarViewModel : IMapFrom<CetakSlipKomisiFakultatifKeluarCommand>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakSlipKomisiFakultatifKeluarViewModel, CetakSlipKomisiFakultatifKeluarCommand>();
        }
    }
}