using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class SaveMutasiKlaimAlokasiCommand : IRequest<MutasiKlaimAlokasi>, IMapFrom<MutasiKlaimAlokasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string kd_grp_pas { get; set; }
        
        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }
        
        public decimal nilai_kl { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveMutasiKlaimAlokasiCommand, MutasiKlaimAlokasi>();
        }
    }

    public class SaveMutasiKlaimAlokasiCommandHandler : IRequestHandler<SaveMutasiKlaimAlokasiCommand, MutasiKlaimAlokasi>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveMutasiKlaimAlokasiCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public SaveMutasiKlaimAlokasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory,
            ILogger<SaveMutasiKlaimAlokasiCommandHandler> logger, ICurrentUserService currentUserService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<MutasiKlaimAlokasi> Handle(SaveMutasiKlaimAlokasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.MutasiKlaimAlokasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts,
                    request.kd_grp_pas, request.kd_rk_pas);
            
                if (entity == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    
                    var mutasiKlaimAlokasi = _mapper.Map<MutasiKlaimAlokasi>(request);

                    dbContext.MutasiKlaimAlokasi.Add(mutasiKlaimAlokasi);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    return mutasiKlaimAlokasi;
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    entity.pst_share = request.pst_share;
                    entity.nilai_kl = request.nilai_kl;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);

                    return entity;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}