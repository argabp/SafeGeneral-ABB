using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Rekanans.Commands
{
    public class SaveRekananCommand : IRequest, IMapFrom<Rekanan>
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
            profile.CreateMap<SaveRekananCommand, Rekanan>();
        }
    }

    public class SaveRekananCommandHandler : IRequestHandler<SaveRekananCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveRekananCommandHandler> _logger;

        public SaveRekananCommandHandler(IDbContextFactory contextFactory, IMapper mapper, 
            IDbConnectionFactory connectionFactory, ILogger<SaveRekananCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveRekananCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var rekanan = dbContext.Rekanan.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                    && w.kd_grp_rk == request.kd_grp_rk
                                                                    && w.kd_rk == request.kd_rk);

                if (rekanan == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
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
                    _mapper.Map(request, rekanan);
                    rekanan.no_ktp ??= string.Empty;
                }

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