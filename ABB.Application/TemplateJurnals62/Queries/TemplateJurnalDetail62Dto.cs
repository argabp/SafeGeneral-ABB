using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals62.Queries
{
    public class TemplateJurnalDetail62Dto : IMapFrom<TemplateJurnalDetail62>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string GlAkun { get; set; }
        public string GlRumus { get; set; }
        public string GlDk { get; set; }
        public int GlUrut { get; set; }
        public string FlagDetail { get; set; }
        public bool? FlagNt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnalDetail62, TemplateJurnalDetail62Dto>();
        }
    }
}