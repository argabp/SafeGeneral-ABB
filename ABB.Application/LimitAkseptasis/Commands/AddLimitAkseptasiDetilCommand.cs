using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class AddLimitAkseptasiDetilCommand : IRequest, IMapFrom<LimitAkseptasiDetil>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal pst_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddLimitAkseptasiDetilCommand, LimitAkseptasiDetil>();
        }
    }

    public class AddLimitAkseptasiDetilCommandHandler : IRequestHandler<AddLimitAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLimitAkseptasiDetilCommandHandler> _logger;

        public AddLimitAkseptasiDetilCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<AddLimitAkseptasiDetilCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitAkseptasiDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasiDetil = _mapper.Map<LimitAkseptasiDetil>(request);

                dbContext.LimitAkseptasiDetil.Add(limitAkseptasiDetil);

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