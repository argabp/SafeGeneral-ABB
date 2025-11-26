using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetRegisterKlaimQuery : IRequest<RegisterKlaimDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
    }

    public class GetRegisterKlaimQueryHandler : IRequestHandler<GetRegisterKlaimQuery, RegisterKlaimDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetRegisterKlaimQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<RegisterKlaimDto> Handle(GetRegisterKlaimQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var registerKlaim = await dbContext.RegisterKlaim.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl);

            return registerKlaim == null ? null : _mapper.Map<RegisterKlaimDto>(registerKlaim);
        }
    }
}