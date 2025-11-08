using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Commands
{
    public class SaveAnalisaDanEvaluasiCommand : IRequest, IMapFrom<AnalisaDanEvaluasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string? ket_anev { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAnalisaDanEvaluasiCommand, AnalisaDanEvaluasi>();
        }
    }

    public class SaveAnalisaDanEvaluasiCommandHandler : IRequestHandler<SaveAnalisaDanEvaluasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAnalisaDanEvaluasiCommandHandler> _logger;

        public SaveAnalisaDanEvaluasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAnalisaDanEvaluasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAnalisaDanEvaluasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var entity = await dbContext.AnalisaDanEvaluasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl);
            
                if (entity == null)
                {
                    var AnalisaDanEvaluasi = _mapper.Map<AnalisaDanEvaluasi>(request);
                    
                    dbContext.AnalisaDanEvaluasi.Add(AnalisaDanEvaluasi);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    entity.ket_anev = request.ket_anev;
                    
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