using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.MutasiKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MutasiKlaim.Models
{
    public class CopyObyekUnderwritingViewModel : MutasiKlaimModel, IMapFrom<CopyObyekUnderwritingCommand>
    {
        public Int16 no_rsk { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyObyekUnderwritingViewModel, CopyObyekUnderwritingCommand>();
        }
    }
}