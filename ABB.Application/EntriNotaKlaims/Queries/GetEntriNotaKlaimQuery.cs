using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriNotas.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaims.Queries
{
    public class GetEntriNotaKlaimQuery : IRequest<EntriNotaKlaim>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetEntriNotaKlaimQueryHandler : IRequestHandler<GetEntriNotaKlaimQuery, EntriNotaKlaim>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEntriNotaKlaimQueryHandler> _logger;

        public GetEntriNotaKlaimQueryHandler(IDbContextFactory dbContextFactory, IMapper mapper,
            ILogger<GetEntriNotaKlaimQueryHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EntriNotaKlaim> Handle(GetEntriNotaKlaimQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var nota = dbContext.NotaKlaim.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                return nota;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}