using System;
using ABB.Application.CancelProsesNotaPremiTreaties.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CancelProsesNotaPremiTreaty.Models
{
    public class CancelProsesNotaPremiTreatyViewModel : IMapFrom<CancelProsesNotaPremiTreatyCommand>
    {
        public DateTime tgl_proses { get; set; }

        public string kd_cob { get; set; }
   
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CancelProsesNotaPremiTreatyViewModel, CancelProsesNotaPremiTreatyCommand>();
        }
    }
}