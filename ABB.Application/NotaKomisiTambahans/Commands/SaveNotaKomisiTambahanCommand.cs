using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaKomisiTambahans.Commands
{
    public class SaveNotaKomisiTambahanCommand : IRequest, IMapFrom<NotaKomisiTambahan>
    {
        public string DatabaseName { get; set; }
        
        public string jns_sb_nt { get; set; }

        public string kd_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? no_nt_lama { get; set; }
        
        public string? no_ref { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public decimal nilai_nt { get; set; }

        public decimal? nilai_lns { get; set; }

        public DateTime tgl_nt { get; set; }

        public string? ket_nt { get; set; }

        public string flag_posting { get; set; }

        public decimal pst_ppn { get; set; }

        public decimal nilai_ppn { get; set; }

        public decimal pst_pph { get; set; }

        public decimal nilai_pph { get; set; }

        public string? tipe_mts { get; set; }

        public string? kd_jns_sor { get; set; }

        public string? kd_rk_sor { get; set; }

        public string? uraian { get; set; }

        public decimal? pst_nt { get; set; }

        public decimal? nilai_prm { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_lain { get; set; }

        public decimal? nilai_lain { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveNotaKomisiTambahanCommand, NotaKomisiTambahan>();
        }
    }

    public class SaveNotaKomisiTambahanCommandHandler : IRequestHandler<SaveNotaKomisiTambahanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveNotaKomisiTambahanCommandHandler> _logger;

        public SaveNotaKomisiTambahanCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveNotaKomisiTambahanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveNotaKomisiTambahanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.NotaKomisiTambahan.FindAsync(
                    request.jns_sb_nt, request.kd_cb, request.jns_tr,
                    request.jns_nt_msk, request.kd_thn, request.kd_bln, request.no_nt_msk,
                    request.jns_nt_kel, request.no_nt_kel);
            
                if (entity == null)
                {
                    var notaKomisiTambahan = _mapper.Map<NotaKomisiTambahan>(request);
                    dbContext.NotaKomisiTambahan.Add(notaKomisiTambahan);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.no_ref = request.no_ref;
                    entity.kd_grp_ttj = request.kd_grp_ttj;
                    entity.kd_rk_ttj = request.kd_rk_ttj;
                    entity.nm_ttj = request.nm_ttj;
                    entity.almt_ttj = request.almt_ttj;
                    entity.kt_ttj = request.kt_ttj;
                    entity.ket_nt = request.ket_nt;
                    entity.tgl_nt = request.tgl_nt;
                    entity.nilai_prm = request.nilai_prm;
                    entity.pst_nt = request.pst_nt;
                    entity.nilai_nt = request.nilai_nt;
                    entity.pst_ppn = request.pst_ppn;
                    entity.nilai_ppn = request.nilai_ppn;
                    entity.pst_pph = request.pst_pph;
                    entity.nilai_pph = request.nilai_pph;
                    entity.pst_lain = request.pst_lain;
                    entity.nilai_lain = request.nilai_lain;
                    entity.flag_posting = request.flag_posting;

                    if(entity.kd_cb.Length != 5)
                        for (int sequence = entity.kd_cb.Length; sequence < 5; sequence++)
                        {
                            entity.kd_cb += " ";
                        }
            
                    if(entity.kd_cob.Length != 2)
                        for (int sequence = entity.kd_cob.Length; sequence < 2; sequence++)
                        {
                            entity.kd_cob += " ";
                        }

                    if(entity.kd_scob.Length != 5)
                        for (int sequence = entity.kd_scob.Length; sequence < 5; sequence++)
                        {
                            entity.kd_scob += " ";
                        }
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}