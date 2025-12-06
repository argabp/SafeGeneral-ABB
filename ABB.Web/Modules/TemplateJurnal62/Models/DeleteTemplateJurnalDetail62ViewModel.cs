using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals62.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal62.Models
{
    public class DeleteTemplateJurnalDetail62ViewModel : IMapFrom<DeleteTemplateJurnalDetail62Command>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }
        public string GlAkun { get; set; }
        public string GlRumus { get; set; }
        public string GlDk { get; set; }
        public short GlUrut { get; set; }
        public string FlagDetail { get; set; }
        public bool? FlagNt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteTemplateJurnalDetail62ViewModel, DeleteTemplateJurnalDetail62Command>();
        }
    }
}