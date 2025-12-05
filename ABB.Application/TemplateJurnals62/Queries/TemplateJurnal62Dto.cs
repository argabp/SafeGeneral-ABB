using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals62.Queries
{
    public class TemplateJurnal62Dto : IMapFrom<TemplateJurnal62>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string NamaJurnal { get; set; }
        public string GridId => $"{Type.Trim()}_{JenisAss.Trim()}";


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnal62, TemplateJurnal62Dto>();
        }
    }
}