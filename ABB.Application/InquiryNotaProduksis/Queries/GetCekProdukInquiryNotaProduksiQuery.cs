using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class GetCekProdukInquiryNotaProduksiQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
         public string JenisAsset { get; set; }

         public string KodeCabang { get; set; }
    }

    public class GetCekProdukInquiryNotaProduksiQueryHandler : IRequestHandler<GetCekProdukInquiryNotaProduksiQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetCekProdukInquiryNotaProduksiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(GetCekProdukInquiryNotaProduksiQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Produksi.AsQueryable();

                    // FILTER CABANG
                    if (!string.IsNullOrEmpty(request.KodeCabang))
                    {
                        query = query.Where(x => x.lok == request.KodeCabang);
                    }

                    // FILTER TANGGAL
                    if (request.StartDate.HasValue && request.EndDate.HasValue)
                    {
                        query = query.Where(x => x.date >= request.StartDate && x.date <= request.EndDate);
                    }
                    else if (request.StartDate.HasValue)
                    {
                        query = query.Where(x => x.date >= request.StartDate);
                    }
                    else if (request.EndDate.HasValue)
                    {
                        query = query.Where(x => x.date <= request.EndDate);
                    }

                    // FILTER JENIS ASSET
                    if (!string.IsNullOrEmpty(request.JenisAsset))
                    {
                        query = query.Where(x => x.jn_ass == request.JenisAsset);
                    }

                   return await query
                    .GroupBy(x => new { x.lok, x.kd_ass2 })
                    .Select(g => new InquiryNotaProduksiDto
                    {
                        lok = g.Key.lok,
                        kd_ass2 = g.Key.kd_ass2,

                        premi = g.Sum(x => (decimal?)(x.premi * x.kurs)),
                        n_rabat = g.Sum(x => (decimal?)(x.n_rabat * x.kurs)),
                        n_bruto = g.Sum(x => (decimal?)(x.n_bruto * x.kurs)),
                        polis = g.Sum(x => (decimal?)((x.polis + x.materai) * x.kurs)),
                        n_komisi = g.Sum(x => (decimal?)(x.n_komisi * x.kurs)),
                        klaim = g.Sum(x => (decimal?)(x.klaim * x.kurs)),
                        h_fee = g.Sum(x => (decimal?)(x.h_fee * x.kurs)),
                        lain = g.Sum(x => (decimal?)(x.lain * x.kurs)),
                        netto = g.Sum(x => (decimal?)(x.netto * x.kurs))
                    })
                    .ToListAsync(cancellationToken);
                       
            }
    }
}
