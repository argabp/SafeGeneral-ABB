using ABB.Application.Common.Interfaces;
using ABB.Application.SlideShows.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.SlideShow.Models
{
    public class SlideShowViewModel : IMapFrom<SaveSlideShowCommand>
    {
        public int Id { get; set; }

        public IFormFile File { get; set; }

        public int Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SlideShowViewModel, SaveSlideShowCommand>();
            profile.CreateMap<SlideShowViewModel, DeleteSlideShowCommand>();
        }
    }
}