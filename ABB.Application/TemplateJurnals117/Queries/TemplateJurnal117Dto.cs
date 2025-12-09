using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class TemplateJurnal117Dto : IMapFrom<TemplateJurnal117>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string NamaJurnal { get; set; }
        public string GridId => $"{Type.Trim()}_{JenisAss.Trim()}";


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnal117, TemplateJurnal117Dto>();
        }
    }
}