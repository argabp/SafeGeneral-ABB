using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class GetDokumenAkseptasiDetilQuery : IRequest<DokumenAkseptasiDetilDto>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }
    }

    public class GetDokumenAkseptasiDetilQueryHandler : IRequestHandler<GetDokumenAkseptasiDetilQuery, DokumenAkseptasiDetilDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDokumenAkseptasiDetilQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<DokumenAkseptasiDetilDto> Handle(GetDokumenAkseptasiDetilQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var dokumenAkseptasiDetil = dbContext.DokumenAkseptasiDetil.FirstOrDefault(w =>
                                                                               w.kd_cob.Trim() == request.kd_cob.Trim()
                                                                               && w.kd_scob.Trim() == request.kd_scob.Trim()
                                                                               && w.kd_dokumen == request.kd_dokumen);

            if (dokumenAkseptasiDetil == null)
                throw new NullReferenceException();

            return _mapper.Map<DokumenAkseptasiDetilDto>(dokumenAkseptasiDetil);
        }
    }
}