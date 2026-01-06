using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.DokumenKlaims.Queries
{
    public class GetDokumenKlaimDetilQuery : IRequest<DokumenKlaimDetilDto>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }
    }

    public class GetDokumenKlaimDetilQueryHandler : IRequestHandler<GetDokumenKlaimDetilQuery, DokumenKlaimDetilDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDokumenKlaimDetilQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<DokumenKlaimDetilDto> Handle(GetDokumenKlaimDetilQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var dokumenKlaimDetil = dbContext.DokumenKlaimDetil.FirstOrDefault(w =>
                w.kd_cob.Trim() == request.kd_cob.Trim()
                && w.kd_scob.Trim() == request.kd_scob.Trim()
                && w.kd_dokumen == request.kd_dokumen);

            if (dokumenKlaimDetil == null)
                throw new NullReferenceException();

            return _mapper.Map<DokumenKlaimDetilDto>(dokumenKlaimDetil);
        }
    }
}