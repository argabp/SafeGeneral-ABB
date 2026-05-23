using System;
using ABB.Application.CetakRekapitulasiProduksiPremiFakultatifKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiPremiFakultatifKeluar.Models
{
    public class CetakRekapitulasiProduksiPremiFakultatifKeluarViewModel : IMapFrom<CetakRekapitulasiProduksiPremiFakultatifKeluarCommand>
    {
        public DateTime periode { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakRekapitulasiProduksiPremiFakultatifKeluarViewModel, CetakRekapitulasiProduksiPremiFakultatifKeluarCommand>();
        }
    }
}