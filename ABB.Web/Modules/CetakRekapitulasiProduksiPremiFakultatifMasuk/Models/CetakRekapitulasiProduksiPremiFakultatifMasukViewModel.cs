using System;
using ABB.Application.CetakRekapitulasiProduksiPremiFakultatifMasuks.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiPremiFakultatifMasuk.Models
{
    public class CetakRekapitulasiProduksiPremiFakultatifMasukViewModel : IMapFrom<CetakRekapitulasiProduksiPremiFakultatifMasukCommand>
    {
        public DateTime periode { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakRekapitulasiProduksiPremiFakultatifMasukViewModel, CetakRekapitulasiProduksiPremiFakultatifMasukCommand>();
        }
    }
}