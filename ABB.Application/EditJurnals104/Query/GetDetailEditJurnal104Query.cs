using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pastikan namespace ini terpanggil

namespace ABB.Application.EditJurnals104.Queries
{
    public class DetailJurnal104Dto
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

    public class GetDetailEditJurnal104Query : IRequest<List<DetailJurnal104Dto>>
    {
        public string DatabaseName { get; set; }
        public string NoBukti { get; set; }
    }

    public class GetDetailEditJurnal104QueryHandler : IRequestHandler<GetDetailEditJurnal104Query, List<DetailJurnal104Dto>>
    {
        private readonly IDbContextPstNota _context;

        public GetDetailEditJurnal104QueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<DetailJurnal104Dto>> Handle(GetDetailEditJurnal104Query request, CancellationToken cancellationToken)
        {
            // LANGSUNG TARIK DARI ENTITY JURNAL62! Sangat bersih dan gampang dibaca.
            var data = await _context.Set<Jurnal62>()
                .AsNoTracking()
                .Where(x => x.GlBukti == request.NoBukti)
                .OrderBy(x => x.GlUrut)
                .Select(x => new DetailJurnal104Dto
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