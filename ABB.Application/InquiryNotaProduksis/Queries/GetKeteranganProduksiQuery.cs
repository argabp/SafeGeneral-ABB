using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class GetKeteranganProduksiQuery : IRequest<List<KeteranganProduksiDto>>
    {
        public string NoNota { get; set; }
    }

    public class GetKeteranganProduksiQueryHandler
        : IRequestHandler<GetKeteranganProduksiQuery, List<KeteranganProduksiDto>>
    {
        private readonly IDbContextPstNota _context;

        public GetKeteranganProduksiQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<KeteranganProduksiDto>> Handle(
            GetKeteranganProduksiQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.KeteranganProduksi
                .Where(x => x.NoNota == request.NoNota)
                .OrderByDescending(x => x.Tanggal)
                .Select(x => new KeteranganProduksiDto
                {
                    Id = x.Id,
                    IdNota = x.IdNota,
                    NoNota = x.NoNota,
                    Tanggal = x.Tanggal,
                    Keterangan = x.Keterangan
                })
                .ToListAsync(cancellationToken);
        }
    }
}
