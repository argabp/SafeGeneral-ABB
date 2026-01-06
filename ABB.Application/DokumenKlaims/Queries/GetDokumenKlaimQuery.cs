using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.DokumenKlaims.Queries
{
    public class GetDokumenKlaimQuery : IRequest<DokumenKlaimDto>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetDokumenKlaimQueryHandler : IRequestHandler<GetDokumenKlaimQuery, DokumenKlaimDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDokumenKlaimQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<DokumenKlaimDto> Handle(GetDokumenKlaimQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var dokumenKlaim = dbContext.DokumenKlaim.FirstOrDefault(approval => approval.kd_cob.Trim() == request.kd_cob.Trim()
                && approval.kd_scob.Trim() == request.kd_scob.Trim());

            if (dokumenKlaim == null)
                throw new NullReferenceException();

            return _mapper.Map<DokumenKlaimDto>(dokumenKlaim);
        }
    }
}