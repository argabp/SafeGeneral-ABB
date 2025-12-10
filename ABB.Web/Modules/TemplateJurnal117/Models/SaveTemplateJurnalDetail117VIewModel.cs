using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals117.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal117.Models
{
    public class SaveTemplateJurnalDetail117ViewModel : IMapFrom<AddTemplateJurnalDetail117Command>
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
            profile.CreateMap<SaveTemplateJurnalDetail117ViewModel, AddTemplateJurnalDetail117Command>();
            profile.CreateMap<SaveTemplateJurnalDetail117ViewModel, EditTemplateJurnalDetail117Command>();
        }
    }
}