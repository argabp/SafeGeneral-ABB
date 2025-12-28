using ABB.Application.Common.Interfaces;
using ABB.Application.Lookups.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lookup.Models
{
    public class SaveLookupViewModel : IMapFrom<AddLookupCommand>
    {
        public string kd_lookup { get; set; }

        public string nm_kategori { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveLookupViewModel, AddLookupCommand>();
            profile.CreateMap<SaveLookupViewModel, EditLookupCommand>();
        }
    }
}