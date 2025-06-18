using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{    
    public class GetDokumensQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetDokumensQueryHandler : IRequestHandler<GetDokumensQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumensQueryHandler> _logger;

        public GetDokumensQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumensQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetDokumensQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_dokumen)) Value, nm_dokumen Text " +
                                                                          "FROM MS_DokumenDetil")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}