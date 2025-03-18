using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiOtherMotorDetailQuery : IRequest<AkseptasiOtherMotorDetailDto>
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
        
        public decimal thn_ptg_kend { get; set; }
    }

    public class GetAkseptasiOtherMotorDetailQueryHandler : IRequestHandler<GetAkseptasiOtherMotorDetailQuery, AkseptasiOtherMotorDetailDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAkseptasiOtherMotorDetailQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiOtherMotorDetailDto> Handle(GetAkseptasiOtherMotorDetailQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var akseptasiResiko = await dbContext.AkseptasiOtherMotorDetail.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                request.no_rsk, request.kd_endt, request.thn_ptg_kend);
                                         
            if (akseptasiResiko == null)
                throw new NullReferenceException();

            return _mapper.Map<AkseptasiOtherMotorDetailDto>(akseptasiResiko);
        }
    }
}