using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PeriodeProses.Commands;
using AutoMapper;

namespace ABB.Web.Modules.PeriodeProses.Models
{
    public class PeriodeProsesViewModel : IMapFrom<AddPeriodeProsesCommand>
    {
        public int Id { get; set; }
        
        public DateTime PeriodeProses { get; set; }

        public bool FlagProses { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PeriodeProsesViewModel, AddPeriodeProsesCommand>();
            profile.CreateMap<PeriodeProsesViewModel, EditPeriodeProsesCommand>();
            profile.CreateMap<PeriodeProsesViewModel, DeletePeriodeProsesCommand>();
        }
    }
}