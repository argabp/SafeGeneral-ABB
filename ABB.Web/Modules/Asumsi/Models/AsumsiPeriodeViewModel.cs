using System;
using ABB.Application.Asumsis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Asumsi.Models
{
    public class AsumsiPeriodeViewModel : IMapFrom<AddAsumsiPeriodeCommand>
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AsumsiPeriodeViewModel, AddAsumsiPeriodeCommand>();
            profile.CreateMap<AsumsiPeriodeViewModel, DeleteAsumsiPeriodeCommand>();
        }
    }
}