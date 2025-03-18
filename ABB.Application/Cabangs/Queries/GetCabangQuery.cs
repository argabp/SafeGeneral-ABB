using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Cabangs.Queries
{
    public class GetCabangQuery : IRequest<CabangDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
    }

    public class GetCabangQueryHandler : IRequestHandler<GetCabangQuery, CabangDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetCabangQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<CabangDto> Handle(GetCabangQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var cabang = dbContext.Cabang.FirstOrDefault(cabang => cabang.kd_cb.Trim() == request.kd_cb.Trim());

            if (cabang == null)
                throw new NullReferenceException();

            return _mapper.Map<CabangDto>(cabang);
        }
    }
}