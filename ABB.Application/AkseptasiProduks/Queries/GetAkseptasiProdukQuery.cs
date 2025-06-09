using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.AkseptasiProduks.Queries
{
    public class GetAkseptasiProdukQuery : IRequest<AkseptasiProdukDto>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetAkseptasiProdukQueryHandler : IRequestHandler<GetAkseptasiProdukQuery, AkseptasiProdukDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetAkseptasiProdukQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<AkseptasiProdukDto> Handle(GetAkseptasiProdukQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var akseptasiProduk = dbContext.AkseptasiProduk.FirstOrDefault(akseptasiProduk => akseptasiProduk.kd_cob.Trim() == request.kd_cob.Trim()
                                                                          && akseptasiProduk.kd_scob.Trim() == request.kd_scob.Trim());

            if (akseptasiProduk == null)
                throw new NullReferenceException();

            return _mapper.Map<AkseptasiProdukDto>(akseptasiProduk);
        }
    }
}