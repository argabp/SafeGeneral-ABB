using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.LimitAkseptasis.Queries
{
    public class GetLimitAkseptasiQuery : IRequest<LimitAkseptasiDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class GetLimitAkseptasiQueryHandler : IRequestHandler<GetLimitAkseptasiQuery, LimitAkseptasiDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetLimitAkseptasiQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<LimitAkseptasiDto> Handle(GetLimitAkseptasiQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var limitAkseptasi = dbContext.LimitAkseptasi.FirstOrDefault(limitAkseptasi => limitAkseptasi.kd_cb.Trim() == request.kd_cb.Trim()
                                                                         && limitAkseptasi.kd_cob.Trim() == request.kd_cob.Trim()
                                                                         && limitAkseptasi.kd_scob.Trim() == request.kd_scob.Trim()
                                                                         && limitAkseptasi.thn == request.thn);

            if (limitAkseptasi == null)
                throw new NullReferenceException();

            return _mapper.Map<LimitAkseptasiDto>(limitAkseptasi);
        }
    }
}