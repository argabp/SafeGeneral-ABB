using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class TemplateJurnalDetail117Dto : IMapFrom<TemplateJurnalDetail117>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string GlAkun { get; set; }
        public string GlRumus { get; set; }
        public string GlDk { get; set; }
        public int GlUrut { get; set; }
        public string FlagDetail { get; set; }
        public bool? FlagNt { get; set; }

        public string GridId { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnalDetail117, TemplateJurnalDetail117Dto>();
        }
    }
}