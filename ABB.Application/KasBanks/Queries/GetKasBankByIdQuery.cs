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
                .FirstOrDefaultAsync(kb => kb.Kode == request.Kode, cancellationToken);
            
            // Konversi dari Entity ke Dto
            var kasBankDto = _mapper.Map<KasBankDto>(entity);

            return kasBankDto;
        }
    }
}