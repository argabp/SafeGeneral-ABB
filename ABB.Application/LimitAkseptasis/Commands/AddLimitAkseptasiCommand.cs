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
    public class AddLimitAkseptasiCommand : IRequest, IMapFrom<LimitAkseptasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public decimal? pst_limit_cb { get; set; }

        public Int16? maks_panel { get; set; }

        public decimal? nilai_kapasitas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddLimitAkseptasiCommand, LimitAkseptasi>();
        }
    }

    public class AddLimitAkseptasiCommandHandler : IRequestHandler<AddLimitAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLimitAkseptasiCommandHandler> _logger;

        public AddLimitAkseptasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<AddLimitAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLimitAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasi = _mapper.Map<LimitAkseptasi>(request);

                dbContext.LimitAkseptasi.Add(limitAkseptasi);

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