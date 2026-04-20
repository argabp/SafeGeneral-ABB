using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesNotaPremiTreaties.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ProsesNotaPremiTreaty.Models
{
    public class ProsesNotaPremiTreatyViewModel : IMapFrom<ProsesNotaPremiTreatyCommand>
    {
        public DateTime tgl_proses { get; set; }
        
        public string kd_cob { get; set; }
   
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesNotaPremiTreatyViewModel, ProsesNotaPremiTreatyCommand>();
        }
    }
}