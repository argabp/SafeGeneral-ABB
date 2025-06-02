using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.EmailTemplates.Commands;
using AutoMapper;

namespace ABB.Web.Modules.EmailTemplate.Models
{
    public class EmailTemplateViewModel : IMapFrom<AddEmailTemplateCommand>
    {
        public int Id { get; set; }

        public Int16 KodeStatus { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmailTemplateViewModel, AddEmailTemplateCommand>();
            profile.CreateMap<Domain.Entities.EmailTemplate, EmailTemplateViewModel>();
            profile.CreateMap<EmailTemplateViewModel, EditEmailTemplateCommand>();
        }
    }
}