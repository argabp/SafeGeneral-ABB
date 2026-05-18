using System;
using ABB.Application.CetakRekapitulasiProduksiTreatyMasuks.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiTreatyMasuk.Models
{
    public class CetakRekapitulasiProduksiTreatyMasukViewModel : IMapFrom<CetakRekapitulasiProduksiTreatyMasukCommand>
    {
        public DateTime periode { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakRekapitulasiProduksiTreatyMasukViewModel, CetakRekapitulasiProduksiTreatyMasukCommand>();
        }
    }
}