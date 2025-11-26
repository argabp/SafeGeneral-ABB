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

    public class GetAllEntriPembayaranKasTempQuery : IRequest<List<EntriPembayaranKasTempDto>>
    {
        public string SearchKeyword { get; set; }
         public string NoVoucher { get; set; }
    }

   
    public class GetAllEntriPembayaranKasTempQueryHandler : IRequestHandler<GetAllEntriPembayaranKasTempQuery, List<EntriPembayaranKasTempDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        
        public GetAllEntriPembayaranKasTempQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EntriPembayaranKasTempDto>> Handle(GetAllEntriPembayaranKasTempQuery request, CancellationToken cancellationToken)
        {
             // Ambil data dasar
            var query = _context.EntriPembayaranKasTemp.AsQueryable();

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
                .ProjectTo<EntriPembayaranKasTempDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return EntriPembayaranKasList;
        }
    }
}