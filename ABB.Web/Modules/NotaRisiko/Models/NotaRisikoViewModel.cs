using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.NotaResiko.Queries;
using AutoMapper;

namespace ABB.Web.Modules.NotaRisiko.Models
{
    public class NotaRisikoViewModel : IMapFrom<GetSourceDataQuery>
    {
        public Int64 Id { get; set; }

        public string TipeTransaksi { get; set; }

        public string KodeMetode { get; set; }

        public DateTime PeriodeAwal { get; set; }

        public DateTime PeriodeAkhir { get; set; }
        
        public bool FlagRelease { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaRisikoViewModel, GetSourceDataQuery>();
        }
    }
}