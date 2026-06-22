using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pastikan namespace ini terpanggil

namespace ABB.Application.EditJurnals117.Queries
{
    public class DetailJurnal117Dto
    {
        public long Id { get; set; }
        public string NoNota { get; set; }
        public string MataUang { get; set; }
        public int NoUrut { get; set; }
        public string DK { get; set; }
        public string KodeAkun { get; set; }
        public decimal NilaiOrg { get; set; }
        public decimal NilaiIdr { get; set; }
    }

    public class GetDetailEditJurnal117Query : IRequest<List<DetailJurnal117Dto>>
    {
        public string DatabaseName { get; set; }
        public string NoBukti { get; set; }
    }

    public class GetDetailEditJurnal117QueryHandler : IRequestHandler<GetDetailEditJurnal117Query, List<DetailJurnal117Dto>>
    {
        private readonly IDbContextPstNota _context;

        public GetDetailEditJurnal117QueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<DetailJurnal117Dto>> Handle(GetDetailEditJurnal117Query request, CancellationToken cancellationToken)
        {
            // LANGSUNG TARIK DARI ENTITY JURNAL117! Sangat bersih dan gampang dibaca.
            var data = await _context.Set<Jurnal117>()
                .AsNoTracking()
                .Where(x => x.GlBukti == request.NoBukti)
                .OrderBy(x => x.GlUrut)
                .Select(x => new DetailJurnal117Dto
                {
                    Id = x.Id,
                    NoNota = x.GlNota,
                    MataUang = x.GlMtu,
                    NoUrut = (int?)x.GlUrut ?? 0,
                    DK = x.GlDk,
                    KodeAkun = x.GlAkun,
                    NilaiOrg = Convert.ToDecimal(x.GlNilaiOrg ?? 0),
                    NilaiIdr = Convert.ToDecimal(x.GlNilaiIdr ?? 0)
                })
                .ToListAsync(cancellationToken);

            return data;
        }
    }
}