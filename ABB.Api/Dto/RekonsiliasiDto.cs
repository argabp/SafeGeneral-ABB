using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesRekonsiliasi.Commands;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ABB.Api.Dto
{
    public class RekonsiliasiDto : IMapFrom<RekonsiliasiNasLifeCommand>
    {
        public string auth_key { get; set; }
        
        public string no_sppa { get; set; }

        public int status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RekonsiliasiDto, RekonsiliasiNasLifeCommand>();
        }
    }
}