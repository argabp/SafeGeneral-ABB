using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetCopyEndorQuery : IRequest<CopyEndorsDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }
    }

    public class GetCopyEndorQueryHandler : IRequestHandler<GetCopyEndorQuery, CopyEndorsDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCopyEndorQueryHandler> _logger;

        public GetCopyEndorQueryHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<GetCopyEndorQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CopyEndorsDto> Handle(GetCopyEndorQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var copyEndors = await dbContext.CopyEndors.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                    request.no_rsk, request.kd_endt);
                                            
                if (copyEndors == null)
                    throw new NullReferenceException();

                return _mapper.Map<CopyEndorsDto>(copyEndors);
            }, _logger);
        }
    }
}