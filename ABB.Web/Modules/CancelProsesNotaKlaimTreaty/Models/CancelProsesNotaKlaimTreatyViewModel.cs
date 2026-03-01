using System;
using ABB.Application.CancelProsesNotaKlaimTreaties.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CancelProsesNotaKlaimTreaty.Models
{
    public class CancelProsesNotaKlaimTreatyViewModel : IMapFrom<CancelProsesNotaKlaimTreatyCommand>
    {
        public DateTime tgl_proses { get; set; }
   
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CancelProsesNotaKlaimTreatyViewModel, CancelProsesNotaKlaimTreatyCommand>();
        }
    }
}