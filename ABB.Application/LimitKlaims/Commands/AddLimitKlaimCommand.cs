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
    public class AddLimitKlaimCommand : IRequest, IMapFrom<LimitKlaim>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddLimitKlaimCommand, LimitKlaim>();
        }
    }

    public class AddLimitKlaimCommandHandler : IRequestHandler<AddLimitKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLimitKlaimCommandHandler> _logger;

        public AddLimitKlaimCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<AddLimitKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaim = _mapper.Map<LimitKlaim>(request);

                dbContext.LimitKlaim.Add(limitKlaim);

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