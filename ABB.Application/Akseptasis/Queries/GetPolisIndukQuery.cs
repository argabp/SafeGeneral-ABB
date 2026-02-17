using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetPolisIndukQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string no_pol_induk { get; set; }
    }

    public class GetPolisIndukQueryHandler : IRequestHandler<GetPolisIndukQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPolisIndukQueryHandler> _logger;

        public GetPolisIndukQueryHandler(IDbConnectionFactory connectionFactory, IMapper mapper,
            ILogger<GetPolisIndukQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetPolisIndukQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
             {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_25", new { request.no_pol_induk })).ToList();
            }, _logger);
        }
    }
}