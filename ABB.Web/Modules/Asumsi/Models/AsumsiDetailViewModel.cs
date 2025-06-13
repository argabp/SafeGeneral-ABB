using System;
using ABB.Application.Asumsis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Asumsi.Models
{
    public class AsumsiDetailViewModel : IMapFrom<AddAsumsiDetailCommand>
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }

        public Int16 Thn { get; set; }

        public decimal Persentase { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AsumsiDetailViewModel, AddAsumsiDetailCommand>();
            profile.CreateMap<AsumsiDetailViewModel, EditAsumsiDetailCommand>();
            profile.CreateMap<AsumsiDetailViewModel, DeleteAsumsiDetailCommand>();
        }
    }
}