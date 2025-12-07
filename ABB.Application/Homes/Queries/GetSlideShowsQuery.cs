using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.Homes.Queries
{
    public class GetSlideShowsQuery : IRequest<List<string>>
    {
        public DateTime tgl_akhir { get; set; }
    }
    
    public class GetSlideShowsQueryHandler : IRequestHandler<GetSlideShowsQuery, List<string>>
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<GetSlideShowsQueryHandler> _logger;
        private readonly IConfiguration _configuration;

        public GetSlideShowsQueryHandler(IDbContext dbContext, ILogger<GetSlideShowsQueryHandler> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }
    
        public async Task<List<string>> Handle(GetSlideShowsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var path = _configuration.GetSection("SlideShow").Value.TrimEnd('/');
                var data = _dbContext.SlideShow.ToList();
                
                foreach (var slide in data)
                {
                    var fullPath = Path.Combine(path, slide.FileName);
                    slide.FileName = fullPath;
                }
                return data.OrderBy(o => o.Order).Select(s => s.FileName).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw;
            }
        }
    }
}