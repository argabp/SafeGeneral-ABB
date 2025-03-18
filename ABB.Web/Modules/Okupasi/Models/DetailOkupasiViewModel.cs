using ABB.Application.Common.Interfaces;
using ABB.Application.Okupasis.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.Okupasi.Models
{
    public class DetailOkupasiViewModel : IMapFrom<SaveDetailOkupasiCommand>
    {
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailOkupasiViewModel, SaveDetailOkupasiCommand>();
            profile.CreateMap<DetailOkupasi, DetailOkupasiViewModel>();
        }
    }
}