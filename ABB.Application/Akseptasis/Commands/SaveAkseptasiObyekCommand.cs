using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiObyekCommand : IRequest, IMapFrom<AkseptasiObyek>
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

        public string kd_grp_oby { get; set; }

        public string desk_oby { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public decimal pst_adj { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiObyekCommand, AkseptasiObyek>();
        }
    }

    public class SaveAkseptasiObyekCommandHandler : IRequestHandler<SaveAkseptasiObyekCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiObyekCommandHandler> _logger;

        public SaveAkseptasiObyekCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiObyekCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiObyekCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiObyek.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_oby);
            
                if (entity == null)
                {
                    var akseptasiObyek = _mapper.Map<AkseptasiObyek>(request);

                    dbContext.AkseptasiObyek.Add(akseptasiObyek);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    _mapper.Map(request, entity);

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