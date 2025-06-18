using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class GetDokumenAkseptasiQuery : IRequest<DokumenAkseptasiDto>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetDokumenAkseptasiQueryHandler : IRequestHandler<GetDokumenAkseptasiQuery, DokumenAkseptasiDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDokumenAkseptasiQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<DokumenAkseptasiDto> Handle(GetDokumenAkseptasiQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var dokumenAkseptasi = dbContext.DokumenAkseptasi.FirstOrDefault(approval => approval.kd_cob.Trim() == request.kd_cob.Trim()
                                                                         && approval.kd_scob.Trim() == request.kd_scob.Trim());

            if (dokumenAkseptasi == null)
                throw new NullReferenceException();

            return _mapper.Map<DokumenAkseptasiDto>(dokumenAkseptasi);
        }
    }
}