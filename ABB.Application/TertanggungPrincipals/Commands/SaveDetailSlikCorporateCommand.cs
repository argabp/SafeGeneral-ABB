using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailSlikCorporateCommand : IRequest, IMapFrom<DetailSlik>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
        
        public string kd_usaha2 { get; set; }
        
        public string tmpt_pendirian { get; set; }
        
        public string akta_pendirian { get; set; }
        
        public DateTime tgl_akta_pendirian { get; set; }
        
        public string akta_berubah_takhir { get; set; }
        
        public DateTime tgl_akta_berubah_takhir { get; set; }
            
        public string? email { get; set; }
        
        public string kd_negara { get; set; }
        
        public string kd_usaha { get; set; }
        
        public string kd_hub { get; set; }
        
        public string langgar_bmpk_pd_pp { get; set; }

        public string lampaui_bmpk_pd_pp { get; set; }

        public string kd_gol_deb { get; set; }
        
        public string? rating_debitur { get; set; }

        public string lembaga_peringkat { get; set; }

        public DateTime tgl_pemeringkatan { get; set; }
        
        public string grp_usaha_deb { get; set; }
        
        public string id_pengurus { get; set; }
        
        public string kd_jns_id_pengurus { get; set; }
        
        public string nm_pengurus { get; set; }
        
        public string Jenis_Kelamin { get; set; }
        
        public string Alamat { get; set; }
        
        public string Kelurahan { get; set; }
        
        public string Kecamatan { get; set; }
        
        public string Kabupaten { get; set; }
        
        public string Kode_Jabatan { get; set; }
        
        public decimal pangsa_kepemilikan { get; set; }
        
        public string sts_pengurus { get; set; }
        
        public DateTime tgl_lap_keu_debitur { get; set; }
        
        public decimal Aset { get; set; }
        
        public decimal Aset_Lancar { get; set; }
        
        public decimal Kas_n_Setara_Kas { get; set; }
        
        public decimal Piutang_Usaha_atau_Pembiayaan { get; set; }
        
        public decimal Inv_atau_Aset_Keu_Lain { get; set; }
        
        public decimal Aset_Lancar_Lain { get; set; }
        
        public decimal Aset_Tak_Lancar { get; set; }
        
        public decimal Piut_Usaha_atau_Pembiayaan { get; set; }
        
        public decimal Inv_atau_Aset_Keu_Lain2 { get; set; }
        
        public decimal Aset_Tak_lancar_Lain { get; set; }
        
        public decimal Liabilitas { get; set; }
        
        public decimal Liabilitas_Jgk_Pdk { get; set; }
        
        public decimal Pinjam_Jgk_Pdk { get; set; }
        
        public decimal Utang_Jgk_Pdk { get; set; }
        
        public decimal Liabilitas_Jgk_Pdk_Lain { get; set; }
        
        public decimal Liabilitas_Jgk_Pjg { get; set; }
        
        public decimal Pinjaman_Jgk_Pjg { get; set; }
        
        public decimal Utang_Jgk_Pjg { get; set; }
        
        public decimal Liabilitas_Jgk_Pjg_Lain { get; set; }
        
        public decimal Ekuitas { get; set; }
        
        public decimal Pendapatan { get; set; }
        
        public decimal Beban_Pkk_Pendapatan { get; set; }
        
        public decimal L_atau_R_Bruto { get; set; }
        
        public decimal Pendapatan_Lain { get; set; }
        
        public decimal Beban_Lain { get; set; }
        
        public decimal L_atau_R_belum_Pjk { get; set; }
        
        public decimal L_atau_R_Thn_Bjl { get; set; }
        public string? no_rek { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailSlikCorporateCommand, DetailSlik>();
        }
    }

    public class SaveDetailSlikCorporateCommandHandler : IRequestHandler<SaveDetailSlikCorporateCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveDetailSlikCorporateCommandHandler> _logger;

        public SaveDetailSlikCorporateCommandHandler(IDbContextFactory contextFactory, IMapper mapper, IDbConnectionFactory connectionFactory,
            ILogger<SaveDetailSlikCorporateCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailSlikCorporateCommand request, CancellationToken cancellationToken)
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
                    data.kd_pekerjaan = string.Empty;
                    data.kd_st_pndk_glr = string.Empty;
                    data.nm_ibu_kdg = string.Empty;
                    dbContext.DetailSlik.Add(data);
                }
                else
                    _mapper.Map(request, detailSlik);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    foreach (SqlError err in sqlEx.Errors)
                    {
                        _logger.LogError($"SQL ERROR {err.Number}: {err.Message}");
                    }
                }
                throw ex.InnerException ?? ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex.InnerException ?? ex;
            }

            return Unit.Value;
        }
    }
}