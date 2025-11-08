using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetDokumenRegisterKlaimQuery : IRequest<DokumenRegisterKlaimDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }
    }

    public class GetDokumenRegisterKlaimQueryHandler : IRequestHandler<GetDokumenRegisterKlaimQuery, DokumenRegisterKlaimDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetDokumenRegisterKlaimQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<DokumenRegisterKlaimDto> Handle(GetDokumenRegisterKlaimQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var registerKlaim = await dbContext.DokumenRegisterKlaim.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.kd_dok);

            return registerKlaim == null ? null : _mapper.Map<DokumenRegisterKlaimDto>(registerKlaim);
        }
    }
}