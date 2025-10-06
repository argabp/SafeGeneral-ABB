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
    public class SaveAkseptasiObyekCITCommand : IRequest, IMapFrom<AkseptasiObyekCIT>
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

        public string? kd_lok_asal { get; set; }

        public string? ket_tujuan { get; set; }

        public DateTime tgl_kirim { get; set; }

        public decimal nilai_srt { get; set; }

        public decimal nilai_kas { get; set; }

        public decimal? pst_rate { get; set; }

        public Int16? jarak { get; set; }

        public string? ket_kirim { get; set; }

        public Int16? jml_wesel { get; set; }

        public decimal? nilai_pa { get; set; }

        public decimal? pst_rate_pa { get; set; }

        public decimal? nilai_prm_pa { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiObyekCITCommand, AkseptasiObyekCIT>();
        }
    }

    public class SaveAkseptasiObyekCITCommandHandler : IRequestHandler<SaveAkseptasiObyekCITCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiObyekCITCommandHandler> _logger;

        public SaveAkseptasiObyekCITCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiObyekCITCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiObyekCITCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiObyekCIT.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_oby);
            
                if (entity == null)
                {
                    var akseptasiObyek = _mapper.Map<AkseptasiObyekCIT>(request);

                    var no_oby = (await _connectionFactory.QueryProc<string>("spe_uw06e_01", new
                        {
                            request.kd_cb, request.kd_cob, request.kd_scob, 
                            request.kd_thn, request.no_aks, request.no_updt,
                            request.no_rsk, request.kd_endt, t_name = "02"
                        }))
                        .First();

                    akseptasiObyek.no_oby = Convert.ToInt16(no_oby.Split(",")[1]);
                    
                    dbContext.AkseptasiObyekCIT.Add(akseptasiObyek);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.kd_lok_asal = request.kd_lok_asal;
                    entity.tgl_kirim = request.tgl_kirim;
                    entity.nilai_srt = request.nilai_srt;
                    entity.nilai_kas = request.nilai_kas;
                    entity.pst_rate = request.pst_rate;
                    entity.jarak = request.jarak;
                    entity.ket_kirim = request.ket_kirim;
                    entity.jml_wesel = request.jml_wesel;
                    entity.nilai_pa = request.nilai_pa;
                    entity.pst_rate_pa = request.pst_rate_pa;
                    entity.nilai_prm_pa = request.nilai_prm_pa;
                    entity.ket_tujuan = request.ket_tujuan;
            
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