using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiOtherMotorDetailCommand : IRequest, IMapFrom<AkseptasiOtherMotorDetail>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public decimal thn_ptg_kend { get; set; }
        
        public decimal nilai_casco { get; set; }

        public decimal nilai_tjh { get; set; }
        
        public decimal nilai_tjp { get; set; }

        public decimal nilai_pap { get; set; }
        
        public decimal nilai_pad { get; set; }

        public decimal nilai_rsk_sendiri { get; set; }

        public decimal nilai_prm_casco { get; set; }

        public decimal nilai_prm_tjh { get; set; }

        public decimal nilai_prm_tjp { get; set; }

        public decimal nilai_prm_pap { get; set; }

        public decimal nilai_prm_pad { get; set; }

        public decimal nilai_prm_hh { get; set; }

        public string? st_deffered { get; set; }

        public decimal? nilai_pap_med { get; set; }
        
        public decimal? nilai_pad_med { get; set; }

        public decimal? nilai_prm_pap_med { get; set; }

        public decimal? nilai_prm_pad_med { get; set; }

        public decimal nilai_prm_aog { get; set; }
        
        public string? kd_jns_ptg_thn { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_rate_banjir { get; set; }

        public byte? stn_rate_banjir { get; set; }

        public decimal? nilai_prm_banjir { get; set; }

        public decimal? nilai_trs { get; set; }

        public decimal? nilai_prm_trs { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherMotorDetailCommand, AkseptasiOtherMotorDetail>();
        }
    }

    public class SaveAkseptasiOtherMotorDetailCommandHandler : IRequestHandler<SaveAkseptasiOtherMotorDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherMotorDetailCommandHandler> _logger;

        public SaveAkseptasiOtherMotorDetailCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherMotorDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherMotorDetailCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherMotorDetail.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.thn_ptg_kend);
            
                if (entity == null)
                {
                    var akseptasiOtherMotorDetail = _mapper.Map<AkseptasiOtherMotorDetail>(request);

                    dbContext.AkseptasiOtherMotorDetail.Add(akseptasiOtherMotorDetail);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    entity.thn_ptg_kend = request.thn_ptg_kend;
                    entity.nilai_casco = request.nilai_casco;
                    entity.nilai_prm_casco = request.nilai_prm_casco;
                    entity.kd_jns_ptg_thn = request.kd_jns_ptg_thn;
                    entity.nilai_rsk_sendiri = request.nilai_rsk_sendiri;
                    entity.nilai_prm_aog = request.nilai_prm_aog;
                    entity.nilai_prm_hh = request.nilai_prm_hh;
                    entity.nilai_tjh = request.nilai_tjh;
                    entity.nilai_prm_tjh = request.nilai_prm_tjh;
                    entity.nilai_tjp = request.nilai_tjp;
                    entity.nilai_prm_tjp = request.nilai_prm_tjp;
                    entity.nilai_pap = request.nilai_pap;
                    entity.nilai_prm_pap = request.nilai_prm_pap;
                    entity.nilai_pad = request.nilai_pad;
                    entity.nilai_prm_pad = request.nilai_prm_pad;
                    entity.nilai_pap_med = request.nilai_pap_med;
                    entity.nilai_prm_pap_med = request.nilai_prm_pap_med;
                    entity.nilai_pad_med = request.nilai_pad_med;
                    entity.nilai_prm_pad_med = request.nilai_prm_pad_med;
                    entity.nilai_prm_trs = request.nilai_prm_trs;
                    entity.nilai_prm_banjir = request.nilai_prm_banjir;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }, _logger);
        }
    }
}