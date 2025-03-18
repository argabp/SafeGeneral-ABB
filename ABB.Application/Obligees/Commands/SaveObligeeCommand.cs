using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Obligees.Commands
{
    public class SaveObligeeCommand : IRequest, IMapFrom<Obligee>
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
            profile.CreateMap<SaveObligeeCommand, Obligee>();
        }
    }

    public class SaveObligeeCommandHandler : IRequestHandler<SaveObligeeCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveObligeeCommandHandler> _logger;

        public SaveObligeeCommandHandler(IDbContextFactory contextFactory, IMapper mapper, 
            IDbConnectionFactory connectionFactory, ILogger<SaveObligeeCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveObligeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var obligee = dbContext.Obligee.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                    && w.kd_grp_rk == request.kd_grp_rk
                                                                    && w.kd_rk == request.kd_rk);

                if (obligee == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    var result = (await _connectionFactory.QueryProc<string>("spe_rf47e_01",
                        new { request.kd_cb, request.kd_grp_rk, kd_rk_induk = "100", request.kd_kota })).FirstOrDefault();

                    var kd_rk = result.Split(",")[1];
                    var newObligee = _mapper.Map<Obligee>(request);
                    newObligee.kd_rk = kd_rk;
                    newObligee.no_ktp = string.Empty;
                    dbContext.Obligee.Add(newObligee);   
                }
                else
                {
                    _mapper.Map(request, obligee);
                    obligee.no_ktp ??= string.Empty;
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