using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriNotas.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Commands
{
    public class SaveEntriNotaCommand : IRequest
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

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string? ket_nt { get; set; }
        
        public string? ket_kwi { get; set; }

        public decimal nilai_nt { get; set; }

        public DateTime tgl_nt { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }

        public decimal pst_ppn { get; set; }

        public decimal? nilai_ppn { get; set; }

        public decimal pst_pph { get; set; }

        public decimal? nilai_pph { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_lain { get; set; }

        public decimal? nilai_lain { get; set; }

        public List<DetailNotaDto> Details { get; set; }

        public string? bayar { get; set; }
    }

    public class SaveEntriNotaCommandHandler : IRequestHandler<SaveEntriNotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveEntriNotaCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveEntriNotaCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveEntriNotaCommandHandler> logger, IMapper mapper)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveEntriNotaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var details = dbContext.DetailNota.Where(w =>
                    w.kd_cb == request.kd_cb && w.jns_tr == request.jns_tr &&
                    w.jns_nt_msk == request.jns_nt_msk && w.kd_thn == request.kd_thn &&
                    w.kd_bln == request.kd_bln && w.no_nt_msk == request.no_nt_msk &&
                    w.jns_nt_kel == request.jns_nt_kel && w.no_nt_kel == request.no_nt_kel).ToList();

                dbContext.DetailNota.RemoveRange(details);

                foreach (var detail in request.Details)
                {
                    dbContext.DetailNota.Add(_mapper.Map<DetailNota>(detail));
                }
                
                var nota = dbContext.Nota.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                nota.kd_grp_ttj = request.kd_grp_ttj;
                nota.kd_rk_ttj = request.kd_rk_ttj;
                nota.nm_ttj = request.nm_ttj;
                nota.almt_ttj = request.almt_ttj;
                nota.kt_ttj = request.kt_ttj;
                nota.ket_nt = request.ket_nt;
                nota.ket_kwi = request.ket_kwi;
                nota.nilai_nt = request.nilai_nt;
                nota.pst_ppn = request.pst_ppn;
                nota.nilai_ppn = request.nilai_ppn;
                nota.pst_pph = request.pst_pph;
                nota.nilai_pph = request.nilai_pph;
                nota.pst_lain = request.pst_lain;
                nota.nilai_lain = request.nilai_lain;
                nota.flag_cancel = request.flag_cancel;
                nota.flag_posting = request.flag_posting;
                nota.bayar = request.bayar;

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