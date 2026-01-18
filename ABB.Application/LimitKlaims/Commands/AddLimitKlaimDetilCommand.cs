using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Commands
{
    public class AddLimitKlaimDetilCommand : IRequest, IMapFrom<LimitKlaimDetil>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }
        
        public decimal nilai_limit_akhir { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddLimitKlaimDetilCommand, LimitKlaimDetil>();
        }
    }

    public class AddLimitKlaimDetilCommandHandler : IRequestHandler<AddLimitKlaimDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLimitKlaimDetilCommandHandler> _logger;

        public AddLimitKlaimDetilCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<AddLimitKlaimDetilCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitKlaimDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaimDetil = _mapper.Map<LimitKlaimDetil>(request);

                dbContext.LimitKlaimDetail.Add(limitKlaimDetil);

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