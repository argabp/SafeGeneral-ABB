using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.LimitAkseptasis.Queries
{
    public class GetLimitAkseptasiDetilQuery : IRequest<LimitAkseptasiDetilDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }
    }

    public class GetLimitAkseptasiDetilQueryHandler : IRequestHandler<GetLimitAkseptasiDetilQuery, LimitAkseptasiDetilDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetLimitAkseptasiDetilQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<LimitAkseptasiDetilDto> Handle(GetLimitAkseptasiDetilQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var limitAkseptasiDetil = dbContext.LimitAkseptasiDetil.FirstOrDefault(approval => approval.kd_cb.Trim() == request.kd_cb.Trim()
                                                                               && approval.kd_cob.Trim() == request.kd_cob.Trim()
                                                                               && approval.kd_scob.Trim() == request.kd_scob.Trim()
                                                                               && approval.thn == request.thn
                                                                               && approval.kd_user == request.kd_user);

            if (limitAkseptasiDetil == null)
                throw new NullReferenceException();

            return _mapper.Map<LimitAkseptasiDetilDto>(limitAkseptasiDetil);
        }
    }
}