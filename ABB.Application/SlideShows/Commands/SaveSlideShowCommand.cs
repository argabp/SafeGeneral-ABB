using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SlideShows.Commands
{
    public class SaveSlideShowCommand : IRequest
    {
        public int Id { get; set; }

        public IFormFile? File { get; set; }

        public int Order { get; set; }
    }

    public class SaveSlideShowCommandHandler : IRequestHandler<SaveSlideShowCommand>
    {
        private readonly IDbContext _context;
        private readonly IProfilePictureHelper _profilePictureHelper;
        private readonly ILogger<SaveSlideShowCommandHandler> _logger;
        private readonly IConfiguration _configuration;

        public SaveSlideShowCommandHandler(IDbContext context, IProfilePictureHelper profilePictureHelper,
            ILogger<SaveSlideShowCommandHandler> logger, IConfiguration configuration)
        {
            _context = context;
            _profilePictureHelper = profilePictureHelper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(SaveSlideShowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.SlideShow.Any(a => a.Id != request.Id && a.Order == request.Order))
                {
                    throw new Exception($"Order {request.Order} is already in use");
                }

                if (request.File != null)
                {
                    if (_context.SlideShow.Any(a => a.Id != request.Id && a.FileName == request.File.FileName))
                    {
                        throw new Exception($"FileName {request.File?.FileName} is already exist");
                    }
                }
                
                var slideShow = _context.SlideShow.FirstOrDefault(w => w.Id == request.Id);
                
                if (slideShow == null)
                {
                    _context.SlideShow.Add(new SlideShow()
                    {
                        FileName = request.File?.FileName ?? string.Empty,
                        Order = request.Order
                    });
                }
                else
                {
                    if (request.File != null)
                    {
                        slideShow.FileName = request.File.FileName;
                    }
                    slideShow.Order = request.Order;
                }
                
                await _context.SaveChangesAsync(cancellationToken);
                
                if (request.File != null)
                {
                    var path = _configuration.GetSection("SlideShow").Value.TrimEnd('/');
                    await _profilePictureHelper.UploadToFolder(request.File, path);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}