using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.LimitKlaims.Queries
{
    public class GetLimitKlaimQuery : IRequest<LimitKlaimDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class GetLimitKlaimQueryHandler : IRequestHandler<GetLimitKlaimQuery, LimitKlaimDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetLimitKlaimQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<LimitKlaimDto> Handle(GetLimitKlaimQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var limitKlaim = dbContext.LimitKlaim.FirstOrDefault(limitKlaim => limitKlaim.kd_cb.Trim() == request.kd_cb.Trim()
                && limitKlaim.kd_cob.Trim() == request.kd_cob.Trim()
                && limitKlaim.kd_scob.Trim() == request.kd_scob.Trim()
                && limitKlaim.thn == request.thn);

            if (limitKlaim == null)
                throw new NullReferenceException();

            return _mapper.Map<LimitKlaimDto>(limitKlaim);
        }
    }
}