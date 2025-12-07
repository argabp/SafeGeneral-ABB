using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SlideShows.Queries
{
    public class GetSlideShowsQuery : IRequest<List<SlideShowDto>>
    {
    }

    public class GetSlideShowsQueryHandler : IRequestHandler<GetSlideShowsQuery, List<SlideShowDto>>
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<GetSlideShowsQueryHandler> _logger;

        public GetSlideShowsQueryHandler(IDbConnection connection, ILogger<GetSlideShowsQueryHandler> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<SlideShowDto>> Handle(GetSlideShowsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return (await _connection.Query<SlideShowDto>(@"
                    SELECT * FROM MS_SlideShow ")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}