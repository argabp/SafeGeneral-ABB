using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.COBs.Queries
{
    public class GetCOBQuery : IRequest<COBDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
    }

    public class GetCOBQueryHandler : IRequestHandler<GetCOBQuery, COBDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetCOBQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<COBDto> Handle(GetCOBQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var cob = dbContext.COB.FirstOrDefault(cob => cob.kd_cob.Trim() == request.kd_cob.Trim());

            if (cob == null)
                throw new NullReferenceException();

            return _mapper.Map<COBDto>(cob);
        }
    }
}