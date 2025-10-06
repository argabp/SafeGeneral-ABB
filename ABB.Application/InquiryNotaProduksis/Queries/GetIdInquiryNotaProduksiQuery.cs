using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InquiryNotaProduksiEntity = ABB.Domain.Entities.Produksi;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class GetInquiryNotaProduksiByIdQuery : IRequest<InquiryNotaProduksiDto>
    {
        public int id { get; set; }
    }

    public class GetInquiryNotaProduksiByIdQueryHandler : IRequestHandler<GetInquiryNotaProduksiByIdQuery, InquiryNotaProduksiDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetInquiryNotaProduksiByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InquiryNotaProduksiDto> Handle(GetInquiryNotaProduksiByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Produksi
                .FirstOrDefaultAsync(p => p.id == request.id, cancellationToken);
            
            // Konversi dari Entity ke Dto
            var InquiryNotaProduksiDto = _mapper.Map<InquiryNotaProduksiDto>(entity);

            return InquiryNotaProduksiDto;
        }
    }
}