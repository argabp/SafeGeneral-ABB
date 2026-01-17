using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailSlikRetailCommand : IRequest, IMapFrom<DetailSlik>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
        
        public string kd_st_pndk_glr { get; set; }
        
        public string? email { get; set; }
        
        public string kd_negara { get; set; }
        
        public string kd_pekerjaan { get; set; }
        
        public string? tmpt_kerja { get; set; }
        
        public string kd_usaha { get; set; }
        
        public string? almt_kerja { get; set; }
        
        public decimal? pdb_thn { get; set; }
        
        public string? kd_hasil { get; set; }
        
        public short? tanggungan { get; set; }
        
        public string kd_hub { get; set; }
        
        public string kd_gol_deb { get; set; }
        
        public string? id_pasangan { get; set; }
        
        public string? nm_pasangan { get; set; }
        
        public DateTime? tgl_lhr_pasangan { get; set; }
        
        public string? pisah_harta { get; set; }
        
        public string langgar_bmpk_pd_pp { get; set; }
        
        public string lampaui_bmpk_pd_pp { get; set; }
        
        public string nm_ibu_kdg { get; set; }
        public string? no_rek { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailSlikRetailCommand, DetailSlik>();
        }
    }

    public class SaveDetailSlikRetailCommandHandler : IRequestHandler<SaveDetailSlikRetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveDetailSlikRetailCommandHandler> _logger;

        public SaveDetailSlikRetailCommandHandler(IDbContextFactory contextFactory, IMapper mapper, IDbConnectionFactory connectionFactory,
            ILogger<SaveDetailSlikRetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailSlikRetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                _connectionFactory.CreateDbConnection(request.DatabaseName);
                
                var detailSlik = dbContext.DetailSlik.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                          && w.kd_grp_rk == request.kd_grp_rk
                                                                          && w.kd_rk == request.kd_rk);

                if (detailSlik == null)
                {
                    var data = _mapper.Map<DetailSlik>(request);
                    data.Alamat = string.Empty;
                    data.Kelurahan = string.Empty;
                    data.Kecamatan = string.Empty;
                    data.Kabupaten = string.Empty;
                    data.Kode_Jabatan = string.Empty;
                    data.sts_pengurus = string.Empty;
                    data.Jenis_Kelamin = string.Empty;
                    data.nm_pengurus = string.Empty;
                    data.kd_jns_id_pengurus = string.Empty;
                    data.id_pengurus = string.Empty;
                    data.grp_usaha_deb = string.Empty;
                    data.lembaga_peringkat = string.Empty;
                    data.akta_berubah_takhir = string.Empty;
                    data.akta_pendirian = string.Empty;
                    data.akta_pendirian = string.Empty;
                    data.tmpt_pendirian = string.Empty;
                    data.kd_usaha2 = string.Empty;
                    data.tgl_akta_pendirian = DateTime.Now;
                    data.tgl_akta_berubah_takhir = DateTime.Now;
                    data.tgl_pemeringkatan = DateTime.Now;
                    data.tgl_lap_keu_debitur = DateTime.Now;
                    dbContext.DetailSlik.Add(data);
                }
                else
                    _mapper.Map(request, detailSlik);

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