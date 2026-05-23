using System;
using ABB.Application.CetakRekapitulasiProduksiTreatyXOLKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiTreatyXOLKeluar.Models
{
    public class CetakRekapitulasiProduksiTreatyXOLKeluarViewModel : IMapFrom<CetakRekapitulasiProduksiTreatyXOLKeluarCommand>
    {
        public DateTime periode { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakRekapitulasiProduksiTreatyXOLKeluarViewModel, CetakRekapitulasiProduksiTreatyXOLKeluarCommand>();
        }
    }
}