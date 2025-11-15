using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class SaveMutasiKlaimObyekCommand : IRequest<MutasiKlaimObyek>, IMapFrom<MutasiKlaimObyek>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_oby { get; set; }

        public string nm_oby { get; set; }

        public decimal nilai_kl { get; set; }

        public Int16? no_rsk { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveMutasiKlaimObyekCommand, MutasiKlaimObyek>();
        }
    }

    public class SaveMutasiKlaimObyekCommandHandler : IRequestHandler<SaveMutasiKlaimObyekCommand, MutasiKlaimObyek>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveMutasiKlaimObyekCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public SaveMutasiKlaimObyekCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory,
            ILogger<SaveMutasiKlaimObyekCommandHandler> logger, ICurrentUserService currentUserService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<MutasiKlaimObyek> Handle(SaveMutasiKlaimObyekCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var entity = await dbContext.MutasiKlaimObyek.FindAsync(request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts, request.no_oby);

                if (entity == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    var no_oby = 1;
                    if(dbContext.MutasiKlaimObyek.Any(w => w.kd_cb == request.kd_cb &&
                                                             w.kd_cob == request.kd_cob &&
                                                             w.kd_scob == request.kd_scob &&
                                                             w.kd_thn == request.kd_thn &&
                                                             w.no_kl == request.no_kl &&
                                                             w.no_mts == request.no_mts))
                    {
                        no_oby = dbContext.MutasiKlaimObyek.Where(w => w.kd_cb == request.kd_cb &&
                                                                     w.kd_cob == request.kd_cob &&
                                                                     w.kd_scob == request.kd_scob &&
                                                                     w.kd_thn == request.kd_thn &&
                                                                     w.no_kl == request.no_kl &&
                                                                     w.no_mts == request.no_mts).Max(m => m.no_oby) + 1;
                    }

                    var mutasiKlaimObyek = _mapper.Map<MutasiKlaimObyek>(request);

                    mutasiKlaimObyek.no_oby = Convert.ToInt16(no_oby);

                    dbContext.MutasiKlaimObyek.Add(mutasiKlaimObyek);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    return mutasiKlaimObyek;
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    entity.nm_oby = request.nm_oby;
                    entity.nilai_kl = request.nilai_kl;
                    entity.nilai_ttl_ptg = request.nilai_ttl_ptg;

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