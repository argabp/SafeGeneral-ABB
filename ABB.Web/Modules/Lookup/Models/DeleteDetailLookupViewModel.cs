using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Lookups.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lookup.Models
{
    public class DeleteDetailLookupViewModel : IMapFrom<DeleteDetailLookupCommand>
    {
        public string kd_lookup { get; set; }

        public int no_lookup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailLookupViewModel, DeleteDetailLookupCommand>();
        }
    }
}