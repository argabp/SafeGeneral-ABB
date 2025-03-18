using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.SCOBs.Queries;
using AutoMapper;
using MediatR;

namespace ABB.Application.PolisInduks.Queries
{
    public class GetPolisIndukQuery : IRequest<PolisIndukDto>
    {
        public string DatabaseName { get; set; }
        public string no_pol_induk { get; set; }
    }

    public class GetPolisIndukQueryHandler : IRequestHandler<GetPolisIndukQuery, PolisIndukDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetPolisIndukQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<PolisIndukDto> Handle(GetPolisIndukQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var polisInduk = dbContext.PolisInduk.FirstOrDefault(polisInduk => polisInduk.no_pol_induk == request.no_pol_induk);

            if (polisInduk == null)
                throw new NullReferenceException();

            return _mapper.Map<PolisIndukDto>(polisInduk);
        }
    }
}