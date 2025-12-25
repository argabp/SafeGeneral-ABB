using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.JurnalMemorials104.Queries;


namespace ABB.Application.PostingJurnalMemorial104.Queries
{
    public class GetAllJurnalMemorial104ByFlagQuery : IRequest<List<JurnalMemorial104Dto>>
    {
        public bool FlagGL { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }  
        public string KodeCabang { get; set; }
    }

    public class GetAllJurnalMemorial104ByFlagQueryHandler : IRequestHandler<GetAllJurnalMemorial104ByFlagQuery, List<JurnalMemorial104Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllJurnalMemorial104ByFlagQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial104Dto>> Handle(GetAllJurnalMemorial104ByFlagQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagGL
            var query = _context.JurnalMemorial104
                .Where(v => v.FlagGL == request.FlagGL)  // filter berdasarkan flag
                .AsQueryable();

            query = query.Where(v => _context.DetailJurnalMemorial104.Any(epb => epb.NoVoucher == v.NoVoucher));

             if (request.FlagGL == false)
            {
                query = query.Where(h => 
                    // A. Cek Apakah Punya Detail
                    _context.DetailJurnalMemorial104.Any(d => d.NoVoucher == h.NoVoucher) 
                    
                    && // DAN
                    
                    // B. Cek Balance (Total Debet == Total Kredit)
                    (
                        _context.DetailJurnalMemorial104
                            .Where(d => d.NoVoucher == h.NoVoucher)
                            .Sum(d => d.NilaiDebet) 
                        == 
                        _context.DetailJurnalMemorial104
                            .Where(d => d.NoVoucher == h.NoVoucher)
                            .Sum(d => d.NilaiKredit)
                    )
                );
            }

            // Jika ada kata kunci pencarian, filter lagi
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword) 
                );
            }

           
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(vk => vk.KodeCabang == request.KodeCabang);
            }

            // Proyeksikan ke DTO
            var JurnalMemorial104List = await query
                .ProjectTo<JurnalMemorial104Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return JurnalMemorial104List;
        }
    }
}
