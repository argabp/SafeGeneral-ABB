using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaims.Commands
{
    public class SaveEntriNotaKlaimCommand : IRequest
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

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string? ket_nt { get; set; }

        public string? ket_kwi { get; set; }

        public decimal nilai_nt { get; set; }
    }

    public class SaveEntriNotaKlaimCommandHandler : IRequestHandler<SaveEntriNotaKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveEntriNotaKlaimCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveEntriNotaKlaimCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveEntriNotaKlaimCommandHandler> logger, IMapper mapper)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveEntriNotaKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var nota = dbContext.NotaKlaim.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                nota.kd_grp_ttj = request.kd_grp_ttj;
                nota.kd_rk_ttj = request.kd_rk_ttj;
                nota.nm_ttj = request.nm_ttj;
                nota.almt_ttj = request.almt_ttj;
                nota.kt_ttj = request.kt_ttj;
                nota.ket_nt = request.ket_nt;
                nota.ket_kwi = request.ket_kwi;
                nota.nilai_nt = request.nilai_nt;

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}