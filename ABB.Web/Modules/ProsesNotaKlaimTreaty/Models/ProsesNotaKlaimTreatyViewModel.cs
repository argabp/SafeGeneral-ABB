using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesNotaKlaimTreaties.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ProsesNotaKlaimTreaty.Models
{
    public class ProsesNotaKlaimTreatyViewModel : IMapFrom<ProsesNotaKlaimTreatyCommand>
    {
        public DateTime tgl_proses { get; set; }
   
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesNotaKlaimTreatyViewModel, ProsesNotaKlaimTreatyCommand>();
        }
    }
}