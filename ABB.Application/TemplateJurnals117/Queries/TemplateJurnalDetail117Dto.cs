using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class TemplateJurnalDetail117Dto : IMapFrom<TemplateJurnalDetail117>
    {
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string gl_akun { get; set; }
        public string gl_rumus { get; set; }
        public string gl_dk { get; set; }
        public short gl_urut { get; set; }
        public string flag_detail { get; set; }
        public bool? flag_nt { get; set; }

        public string GridId { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnalDetail117, TemplateJurnalDetail117Dto>();
        }
    }
}