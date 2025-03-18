using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.SCOBs.Queries
{
    public class GetSCOBQuery : IRequest<SCOBDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetSCOBQueryHandler : IRequestHandler<GetSCOBQuery, SCOBDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetSCOBQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<SCOBDto> Handle(GetSCOBQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var scob = dbContext.SCOB.FirstOrDefault(scob => scob.kd_cob == request.kd_cob && scob.kd_scob == scob.kd_scob);

            if (scob == null)
                throw new NullReferenceException();

            return _mapper.Map<SCOBDto>(scob);
        }
    }
}