using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KasBankEntity = ABB.Domain.Entities.KasBank;

namespace ABB.Application.KasBanks.Queries
{
    public class GetKasBankByIdQuery : IRequest<KasBankDto>
    {
        public string Kode { get; set; }
        public string KodeCabang { get; set; }
        public string Tipe { get; set; }
    }

    public class GetKasBankByIdQueryHandler : IRequestHandler<GetKasBankByIdQuery, KasBankDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetKasBankByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       public async Task<KasBankDto> Handle(GetKasBankByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.KasBank
                    .FirstOrDefaultAsync(kb =>
                        kb.Kode == request.Kode &&
                        kb.KodeCabang == request.KodeCabang &&
                        kb.TipeKasBank == request.Tipe,
                        cancellationToken);

                if (entity == null)
                    return null;

                return _mapper.Map<KasBankDto>(entity);
            }
    }
}