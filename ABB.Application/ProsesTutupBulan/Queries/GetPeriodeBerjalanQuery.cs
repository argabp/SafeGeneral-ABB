using System.Collections.Generic; // Butuh ini untuk List<>
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesTutupBulan.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.ProsesTutupBulan.Queries
{
    // Return-nya sekarang List<...Dto>
    public class GetDaftarPeriodeQuery : IRequest<List<ProsesTutupBulanDto>>
    {
    }

    public class GetDaftarPeriodeQueryHandler : IRequestHandler<GetDaftarPeriodeQuery, List<ProsesTutupBulanDto>>
    {
        private readonly IDbContextPstNota _context;

        public GetDaftarPeriodeQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<ProsesTutupBulanDto>> Handle(GetDaftarPeriodeQuery request, CancellationToken cancellationToken)
            {
                var listPeriode = await _context.EntriPeriode
                    .Where(x => x.FlagClosing != "Y")
                    .OrderBy(x => x.ThnPrd).ThenBy(x => x.BlnPrd)
                    .ToListAsync(cancellationToken);

                // Mapping ke DTO
                var result = listPeriode.Select(p => new ProsesTutupBulanDto
                {
                    ThnPrd = p.ThnPrd.ToString("0"),
                    BlnPrd = p.BlnPrd.ToString("00"),
                    TglMul = p.TglMul?.ToString("dd-MM-yyyy"),
                    TglAkh = p.TglAkh?.ToString("dd-MM-yyyy"),
                    Status = "OPEN",
                    IsReadyToClose = true
                }).ToList();

                // --- TAMBAHKAN INI ---
                return result; 
            }
    }
}