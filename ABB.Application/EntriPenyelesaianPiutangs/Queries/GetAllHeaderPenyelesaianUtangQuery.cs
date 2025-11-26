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

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
         public class GetAllHeaderPenyelesaianUtangQuery : IRequest<List<HeaderPenyelesaianUtangDto>>
         {
                  public string SearchKeyword { get; set; }
                  public string KodeCabang { get; set; }
                  public bool FlagFinal { get; set; }
         }

         public class GetAllHeaderPenyelesaianUtangQueryHandler : IRequestHandler<GetAllHeaderPenyelesaianUtangQuery, List<HeaderPenyelesaianUtangDto>>
         {
              private readonly IDbContextPstNota _context;
              private readonly IMapper _mapper;

              public GetAllHeaderPenyelesaianUtangQueryHandler(IDbContextPstNota context, IMapper mapper)
              {
                       _context = context;
                       _mapper = mapper;
              }

              public async Task<List<HeaderPenyelesaianUtangDto>> Handle(GetAllHeaderPenyelesaianUtangQuery request, CancellationToken cancellationToken)
              {
                       var query = _context.HeaderPenyelesaianUtang.AsQueryable();

                       // 1. FILTER BERDASARKAN KATA KUNCI (SEARCH KEYWORD)
                       if (!string.IsNullOrEmpty(request.SearchKeyword))
                       {
                            var keyword = request.SearchKeyword.ToLower();

                            query = query.Where(h =>
                                     h.NomorBukti.ToLower().Contains(keyword) ||
                                     h.Keterangan.ToLower().Contains(keyword) ||
                                     h.KodeAkun.ToLower().Contains(keyword)
                            );
                       }
             
// ----------------------------------------------------------------------

                       // 2. FILTER BERDASARKAN KODE CABANG
                       if (!string.IsNullOrEmpty(request.KodeCabang))
                       {
                // Asumsi properti entitas di HeaderPenyelesaianUtang adalah KodeCabang
                            query = query.Where(h => h.KodeCabang == request.KodeCabang);
                       }

                       // 3. FILTER BERDASARKAN FLAG FINAL
            // FlagFinal (bool) di request harus dipetakan ke field di entitas (biasanya string "Y"/"N" atau bool)
            // Di sini saya asumsikan entitas HeaderPenyelesaianUtang memiliki properti FlagFinal yang bertipe bool.
            // Jika FlagFinal di entitas Anda bertipe string "Y" atau "N", ubah baris di bawah ini.
                       query = query.Where(h => h.FlagFinal == request.FlagFinal);
            
// ----------------------------------------------------------------------

                       // PROJEKSI DAN EKSEKUSI
                       return await query
                            .ProjectTo<HeaderPenyelesaianUtangDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
              }
         }
}