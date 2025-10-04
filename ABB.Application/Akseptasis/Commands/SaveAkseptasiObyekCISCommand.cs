using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiObyekCISCommand : IRequest, IMapFrom<AkseptasiObyekCIS>
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

        public Int16 no_oby { get; set; }

        public DateTime tgl_oby { get; set; }

        public decimal nilai_saldo { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiObyekCISCommand, AkseptasiObyekCIS>();
        }
    }

    public class SaveAkseptasiObyekCISCommandHandler : IRequestHandler<SaveAkseptasiObyekCISCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiObyekCISCommandHandler> _logger;

        public SaveAkseptasiObyekCISCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiObyekCISCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiObyekCISCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiObyekCIS.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_oby);

                var dataCis = dbContext.AkseptasiObyekCIS.Where(w => w.kd_cb == request.kd_cb
                                                                    && w.kd_cob == request.kd_cob &&
                                                                    w.kd_scob == request.kd_scob &&
                                                                    w.kd_thn == request.kd_thn
                                                                    && w.no_aks == request.no_aks &&
                                                                    w.no_updt == request.no_updt &&
                                                                    w.no_rsk == request.no_rsk
                                                                    && w.kd_endt == request.kd_endt &&
                                                                    w.no_oby == request.no_oby);

                if (dataCis.Any())
                {
                    request.tgl_oby = dataCis.Max(w => w.tgl_oby).AddDays(1);
                }

                if (entity == null)
                {
                    var akseptasiObyek = _mapper.Map<AkseptasiObyekCIS>(request);

                    var no_oby = (await _connectionFactory.QueryProc<string>("spe_uw06e_01", new
                        {
                            request.kd_cb, request.kd_cob, request.kd_scob, 
                            request.kd_thn, request.no_aks, request.no_updt,
                            request.no_rsk, request.kd_endt, t_name = "04"
                        }))
                        .First();

                    akseptasiObyek.no_oby = Convert.ToInt16(no_oby.Split(",")[1]);
                    
                    dbContext.AkseptasiObyekCIS.Add(akseptasiObyek);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.tgl_oby = request.tgl_oby;
                    entity.nilai_saldo = request.nilai_saldo;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                await _connectionFactory.QueryProc<string>("spe_uw02e_20", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_aks, request.no_updt,
                    request.no_rsk, request.kd_endt
                });
                    
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