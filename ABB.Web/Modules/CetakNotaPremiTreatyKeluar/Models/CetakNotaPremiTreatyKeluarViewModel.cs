using System;
using ABB.Application.CetakNotaPremiTreatyKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakNotaPremiTreatyKeluar.Models
{
    public class CetakNotaPremiTreatyKeluarViewModel : IMapFrom<CetakNotaPremiTreatyKeluarCommand>
    {
        public DateTime periode { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakNotaPremiTreatyKeluarViewModel, CetakNotaPremiTreatyKeluarCommand>();
        }
    }
}