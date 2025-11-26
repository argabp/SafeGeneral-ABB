using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LKTs.Queries
{
    public class GetLKTQuery : IRequest<LKT>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string st_tipe_dla { get; set; }

        public Int16 no_dla { get; set; }
    }

    public class GetLKTQueryHandler : IRequestHandler<GetLKTQuery, LKT>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLKTQuery> _logger;

        public GetLKTQueryHandler(IDbContextFactory dbContextFactory, IMapper mapper,
            ILogger<GetLKTQuery> logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LKT> Handle(GetLKTQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var lkt = dbContext.LKT.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts, request.st_tipe_dla, request.no_dla);

                return lkt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}