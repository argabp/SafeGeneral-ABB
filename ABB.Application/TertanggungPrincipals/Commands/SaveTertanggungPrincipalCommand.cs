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
    public class SaveTertanggungPrincipalCommand : IRequest, IMapFrom<Rekanan>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }

        public string? nm_rk { get; set; }

        public string? almt { get; set; }

        public string? kt { get; set; }

        public string? kd_pos { get; set; }

        public string? no_tlp { get; set; }

        public string? no_fax { get; set; }

        public string? npwp { get; set; }

        public string? flag_sic { get; set; }

        public string? no_ktp { get; set; }

        public string? person { get; set; }

        public string? person_tlp { get; set; }

        public string? person_tlp_rmh { get; set; }

        public string? person_tlp_ktr { get; set; }

        public string? jbt_person { get; set; }

        public string? kd_rk_ref { get; set; }

        public DateTime? tgl_berdiri { get; set; }

        public string? no_akta { get; set; }

        public string? no_rekening { get; set; }

        public string? nm_dirut { get; set; }

        public string? kd_rk_induk { get; set; }

        public string? kd_kota { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveTertanggungPrincipalCommand, Rekanan>();
        }
    }

    public class SaveTertanggungPrincipalCommandHandler : IRequestHandler<SaveTertanggungPrincipalCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveTertanggungPrincipalCommandHandler> _logger;

        public SaveTertanggungPrincipalCommandHandler(IDbContextFactory contextFactory, IMapper mapper, 
            IDbConnectionFactory connectionFactory, ILogger<SaveTertanggungPrincipalCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveTertanggungPrincipalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc<string>("sp_Cekrf03",
                    new { request.nm_rk, request.almt, request.kd_kota, request.flag_sic });
                
                var rekanan = dbContext.Rekanan.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                    && w.kd_grp_rk == request.kd_grp_rk
                                                                    && w.kd_rk == request.kd_rk);
                
                if (rekanan == null)
                {
                    request.kd_kota = "00";
                    
                    var result = (await _connectionFactory.QueryProc<string>("spe_rf03e_01",
                        new { request.kd_cb, request.kd_grp_rk, kd_rk_induk = "100", request.kd_kota })).FirstOrDefault();

                    var kd_rk = result.Split(",")[1];
                    var newRekanan = _mapper.Map<Rekanan>(request);
                    newRekanan.kd_rk = kd_rk;
                    newRekanan.no_ktp = string.Empty;
                    dbContext.Rekanan.Add(newRekanan);   
                }
                else
                {
                    rekanan.nm_rk = request.nm_rk;
                    rekanan.kt = request.kt;
                    rekanan.almt = request.almt;
                    rekanan.flag_sic = request.flag_sic;
                    rekanan.no_fax = request.no_fax;
                }

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