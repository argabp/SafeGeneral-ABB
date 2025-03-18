using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetResikoQuery : IRequest<AkseptasiResiko>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetResikoQueryHandler : IRequestHandler<GetResikoQuery, AkseptasiResiko>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetResikoQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiResiko> Handle(GetResikoQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var akseptasiResiko = dbContext.AkseptasiResiko.FirstOrDefault(a => a.kd_cb == request.kd_cb && 
                a.kd_cob == request.kd_cob && a.kd_scob == request.kd_scob && a.kd_thn == request.kd_thn 
            && a.no_aks == request.no_aks && a.no_updt == request.no_updt);
                                         
            if (akseptasiResiko == null)
                throw new NullReferenceException();

            return akseptasiResiko;
        }
    }
}