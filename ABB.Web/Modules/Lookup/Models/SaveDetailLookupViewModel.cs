using ABB.Application.Common.Interfaces;
using ABB.Application.Lookups.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lookup.Models
{
    public class SaveDetailLookupViewModel : IMapFrom<AddDetailLookupCommand>
    {
        public string kd_lookup { get; set; }

        public int no_lookup { get; set; }

        public string nm_detail_lookup { get; set; }

        public string flag_lookup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailLookupViewModel, AddDetailLookupCommand>();
            profile.CreateMap<SaveDetailLookupViewModel, EditDetailLookupCommand>();
        }
    }
}