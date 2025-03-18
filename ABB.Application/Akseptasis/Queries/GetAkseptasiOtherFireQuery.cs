using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiOtherFireQuery : IRequest<AkseptasiOtherFireDto?>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }
    }

    public class GetAkseptasiOtherFireQueryHandler : IRequestHandler<GetAkseptasiOtherFireQuery, AkseptasiOtherFireDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAkseptasiOtherFireQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiOtherFireDto?> Handle(GetAkseptasiOtherFireQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var akseptasiResiko = await dbContext.AkseptasiOtherFire.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                request.no_rsk, request.kd_endt);

            return akseptasiResiko == null ? null : _mapper.Map<AkseptasiOtherFireDto>(akseptasiResiko);
        }
    }
}