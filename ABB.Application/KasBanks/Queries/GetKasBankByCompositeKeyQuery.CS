using ABB.Application.Common.Interfaces;
using ABB.Application.KasBanks.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.KasBanks.Queries
{
    // =========================
    // QUERY
    // =========================
    public class GetKasBankByCompositeKeyQuery : IRequest<KasBankDto>
    {
        public string KodeCabang { get; set; }
        public string TipeKasBank { get; set; }
        public string Kode { get; set; }
    }

    // =========================
    // HANDLER
    // =========================
    public class GetKasBankByCompositeKeyQueryHandler
        : IRequestHandler<GetKasBankByCompositeKeyQuery, KasBankDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetKasBankByCompositeKeyQueryHandler(
            IDbContextPstNota context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<KasBankDto> Handle(
            GetKasBankByCompositeKeyQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.KodeCabang) ||
                string.IsNullOrEmpty(request.TipeKasBank) ||
                string.IsNullOrEmpty(request.Kode))
            {
                return null;
            }

            var entity = await _context.KasBank
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.KodeCabang == request.KodeCabang &&
                    x.TipeKasBank == request.TipeKasBank &&
                    x.Kode == request.Kode,
                    cancellationToken);

            if (entity == null)
                return null;

            return _mapper.Map<KasBankDto>(entity);
        }
    }
}
