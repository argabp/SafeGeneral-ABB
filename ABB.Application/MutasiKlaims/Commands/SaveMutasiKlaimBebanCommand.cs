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
    public class SaveMutasiKlaimBebanCommand : IRequest<MutasiKlaimBeban>, IMapFrom<MutasiKlaimBeban>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_urut { get; set; }

        public string st_jns { get; set; }
        
        public string ket_jns { get; set; }
        
        public string kd_mtu { get; set; }

        public decimal nilai_jns_org { get; set; }
        
        public decimal nilai_jns { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveMutasiKlaimBebanCommand, MutasiKlaimBeban>();
        }
    }

    public class SaveMutasiKlaimBebanCommandHandler : IRequestHandler<SaveMutasiKlaimBebanCommand, MutasiKlaimBeban>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveMutasiKlaimBebanCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public SaveMutasiKlaimBebanCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory,
            ILogger<SaveMutasiKlaimBebanCommandHandler> logger, ICurrentUserService currentUserService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<MutasiKlaimBeban> Handle(SaveMutasiKlaimBebanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.MutasiKlaimBeban.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts, request.no_urut);
            
                if (entity == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    
                    var no_urut = 1;
                    if(dbContext.MutasiKlaimBeban.Any(w => w.kd_cb == request.kd_cb &&
                                                           w.kd_cob == request.kd_cob &&
                                                           w.kd_scob == request.kd_scob &&
                                                           w.kd_thn == request.kd_thn &&
                                                           w.no_kl == request.no_kl &&
                                                           w.no_mts == request.no_mts))
                    {
                        no_urut = dbContext.MutasiKlaimBeban.Where(w => w.kd_cb == request.kd_cb &&
                                                                        w.kd_cob == request.kd_cob &&
                                                                        w.kd_scob == request.kd_scob &&
                                                                        w.kd_thn == request.kd_thn &&
                                                                        w.no_kl == request.no_kl &&
                                                                        w.no_mts == request.no_mts).Max(m => m.no_urut) + 1;
                    }
                    
                    var mutasiKlaimBeban = _mapper.Map<MutasiKlaimBeban>(request);
                
                    mutasiKlaimBeban.no_urut = Convert.ToInt16(no_urut);

                    dbContext.MutasiKlaimBeban.Add(mutasiKlaimBeban);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    return mutasiKlaimBeban;
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    entity.st_jns = request.st_jns;
                    entity.ket_jns = request.ket_jns;
                    entity.kd_mtu = request.kd_mtu;
                    entity.nilai_jns_org = request.nilai_jns_org;
                    entity.nilai_jns = request.nilai_jns;
                    
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