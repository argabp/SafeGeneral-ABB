using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SlideShows.Commands
{
    public class DeleteSlideShowCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteSlideShowCommandHandler : IRequestHandler<DeleteSlideShowCommand>
    {
        private readonly IDbContext _context;
        private readonly ILogger<DeleteSlideShowCommandHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _root;

        public DeleteSlideShowCommandHandler(IDbContext context, ILogger<DeleteSlideShowCommandHandler> logger,
            IConfiguration configuration, IHostEnvironment root)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _root = root;
        }

        public async Task<Unit> Handle(DeleteSlideShowCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var slideShow = _context.SlideShow.FirstOrDefault(w => w.Id == request.Id);

                if (slideShow != null)
                {
                    _context.SlideShow.Remove(slideShow);

                    await _context.SaveChangesAsync(cancellationToken);
                
                    var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot");
                    var path = _configuration.GetSection("SlideShow").Value.TrimEnd('/');
                    var root = Path.Combine(wwwroot, path.TrimStart('/'));
                    var fullPath = Path.Combine(root, slideShow.FileName);
                    
                    if(File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}