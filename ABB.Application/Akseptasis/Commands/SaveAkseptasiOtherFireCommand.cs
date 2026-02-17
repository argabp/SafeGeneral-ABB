using System;
using System.Linq;
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
    public class SaveAkseptasiOtherFireCommand : IRequest, IMapFrom<AkseptasiOtherFire>
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

        public string? kd_zona { get; set; }

        public string kd_prop { get; set; }

        public string? kd_lok_rsk { get; set; }

        public string almt_rsk { get; set; }

        public string kt_rsk { get; set; }

        public string? kd_pos_rsk { get; set; }

        public string? kd_penerangan { get; set; }

        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public string? kategori_gd { get; set; }

        public Int16? umur_gd { get; set; }

        public Int16? jml_lantai { get; set; }

        public string? ket_okup { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_kab { get; set; }

        public string? kd_kec { get; set; }

        public string? kd_kel { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherFireCommand, AkseptasiOtherFire>();
        }
    }

    public class SaveAkseptasiOtherFireCommandHandler : IRequestHandler<SaveAkseptasiOtherFireCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherFireCommandHandler> _logger;

        public SaveAkseptasiOtherFireCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherFireCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherFireCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherFire.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                var result = (await _connectionFactory.QueryProc<string>("spe_uw02e_97", new
                {
                    request.kd_pos_rsk, request.kd_lok_rsk
                })).First();

                if (!string.IsNullOrWhiteSpace(result))
                    throw new Exception(result);
                
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherFire>(request);

                    dbContext.AkseptasiOtherFire.Add(akseptasiOther);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.kd_zona = request.kd_zona;
                    entity.kd_lok_rsk = request.kd_lok_rsk;
                    entity.almt_rsk = request.almt_rsk;
                    entity.kt_rsk = request.kt_rsk;
                    entity.kd_pos_rsk = request.kd_pos_rsk;
                    entity.kd_penerangan = request.kd_penerangan;
                    entity.kd_prop = request.kd_prop;
                    entity.kd_kls_konstr = request.kd_kls_konstr;
                    entity.kd_kab = request.kd_kab;
                    entity.kd_okup = request.kd_okup;
                    entity.kd_kec = request.kd_kec;
                    entity.ket_okup = request.ket_okup;
                    entity.kd_kel = request.kd_kel;
                    entity.kategori_gd = request.kategori_gd;
                    entity.umur_gd = request.umur_gd;
                    entity.jml_lantai = request.jml_lantai;

                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }, _logger);
        }
    }
}