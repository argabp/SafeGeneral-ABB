using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKomisiTambahans.Queries
{
    public class GetCancelPostingNotaKomisiTambahanQuery : IRequest<List<CancelPostingNotaKomisiTambahanDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetCancelPostingNotaKomisiTambahanQueryHandler : IRequestHandler<GetCancelPostingNotaKomisiTambahanQuery, List<CancelPostingNotaKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCancelPostingNotaKomisiTambahanQueryHandler> _logger;

        public GetCancelPostingNotaKomisiTambahanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCancelPostingNotaKomisiTambahanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CancelPostingNotaKomisiTambahanDto>> Handle(GetCancelPostingNotaKomisiTambahanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CancelPostingNotaKomisiTambahanDto>("SELECT * FROM fn05 WHERE flag_posting = 'Y' AND jns_tr = 'P' AND jns_nt_msk = '0' AND jns_nt_kel IN ('C', 'D')")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}