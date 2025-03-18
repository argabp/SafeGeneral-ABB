using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiOtherPAQuery : IRequest<AkseptasiOtherPADto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public int no_updt { get; set; }

        public int no_rsk { get; set; }

        public string kd_endt { get; set; }
    }

    public class GetAkseptasiOtherPAQueryHandler : IRequestHandler<GetAkseptasiOtherPAQuery, AkseptasiOtherPADto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAkseptasiOtherPAQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiOtherPADto> Handle(GetAkseptasiOtherPAQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var akseptasiResiko = await dbContext.AkseptasiOtherPA.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                request.no_rsk, request.kd_endt);

            return akseptasiResiko == null ? null : _mapper.Map<AkseptasiOtherPADto>(akseptasiResiko);
        }
    }
}