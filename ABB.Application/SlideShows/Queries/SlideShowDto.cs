using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.SlideShows.Queries
{
    public class SlideShowDto : IMapFrom<SlideShow>
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public int Order { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SlideShow, SlideShowDto>();
        }
    }
}