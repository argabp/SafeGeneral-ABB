using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.LimitKlaims.Queries
{
    public class GetLimitKlaimDetilQuery : IRequest<LimitKlaimDetilDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }
    }

    public class GetLimitKlaimDetilQueryHandler : IRequestHandler<GetLimitKlaimDetilQuery, LimitKlaimDetilDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetLimitKlaimDetilQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<LimitKlaimDetilDto> Handle(GetLimitKlaimDetilQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var limitKlaimDetil = dbContext.LimitKlaimDetail.FirstOrDefault(approval => approval.kd_cb.Trim() == request.kd_cb.Trim()
                && approval.kd_cob.Trim() == request.kd_cob.Trim()
                && approval.kd_scob.Trim() == request.kd_scob.Trim()
                && approval.thn == request.thn
                && approval.kd_user == request.kd_user);

            if (limitKlaimDetil == null)
                throw new NullReferenceException();

            return _mapper.Map<LimitKlaimDetilDto>(limitKlaimDetil);
        }
    }
}