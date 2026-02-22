using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetSCOBByKodeCabangQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
    }

    public class GetSCOBByKodeCabangQueryHandler : IRequestHandler<GetSCOBByKodeCabangQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbContext _dbContext;
        private readonly ILogger<GetSCOBByKodeCabangQueryHandler> _logger;

        public GetSCOBByKodeCabangQueryHandler(IDbConnectionFactory connectionFactory, 
            IDbContext dbContext, ILogger<GetSCOBByKodeCabangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetSCOBByKodeCabangQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var cabang = _dbContext.Cabang.Find(request.kd_cb);

                if(cabang == null)
                {
                    throw new NullReferenceException("Cabang tidak ditemukan");
                }

                _connectionFactory.CreateDbConnection(cabang.database_name);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    @"SELECT RTRIM(LTRIM(kd_scob)) Value, nm_scob Text FROM rf05 WHERE kd_cob = @kd_cob",
                    new { request.kd_cob })).ToList();
            }, _logger);
        }
    }
}