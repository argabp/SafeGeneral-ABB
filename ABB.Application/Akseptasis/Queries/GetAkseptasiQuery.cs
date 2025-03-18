using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.PolisInduks.Queries;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiQuery : IRequest<AkseptasiDto?>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetAkseptasiQueryHandler : IRequestHandler<GetAkseptasiQuery, AkseptasiDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAkseptasiQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiDto?> Handle(GetAkseptasiQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var akseptasi = await dbContext.Akseptasi.FindAsync(request.kd_cb, 
                                             request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt);

            return akseptasi == null ? null : _mapper.Map<AkseptasiDto>(akseptasi);
        }
    }
}