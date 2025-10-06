using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ABB.Application.EntriPembayaranKass.Queries
{

    public class GetAllEntriPembayaranKasQuery : IRequest<List<EntriPembayaranKasDto>>
    {
        public string SearchKeyword { get; set; }
         public string NoVoucher { get; set; }
    }

   
    public class GetAllEntriPembayaranKasQueryHandler : IRequestHandler<GetAllEntriPembayaranKasQuery, List<EntriPembayaranKasDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        
        public GetAllEntriPembayaranKasQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EntriPembayaranKasDto>> Handle(GetAllEntriPembayaranKasQuery request, CancellationToken cancellationToken)
        {
             // Ambil data dasar
            var query = _context.EntriPembayaranKas.AsQueryable();

            // JIKA ADA KATA KUNCI, LAKUKAN FILTER
             if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(pb => 
                    pb.NoVoucher.ToLower().Contains(keyword) ||
                    pb.KodeAkun.ToLower().Contains(keyword)
                );
            }

            if (!string.IsNullOrEmpty(request.NoVoucher))
            {
                query = query.Where(pb => pb.NoVoucher == request.NoVoucher);
            }

            // Lanjutkan proses seperti biasa dengan data yang sudah difilter
            var EntriPembayaranKasList = await query
                .ProjectTo<EntriPembayaranKasDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return EntriPembayaranKasList;
        }
    }
}