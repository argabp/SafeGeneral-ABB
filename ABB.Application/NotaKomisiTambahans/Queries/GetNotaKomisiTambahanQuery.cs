using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNotaKomisiTambahanQuery : IRequest<NotaKomisiTambahanDto>
    {
        public string DatabaseName { get; set; }
        public string jns_sb_nt { get; set; }

        public string kd_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetNotaKomisiTambahanQueryHandler : IRequestHandler<GetNotaKomisiTambahanQuery, NotaKomisiTambahanDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetNotaKomisiTambahanQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<NotaKomisiTambahanDto> Handle(GetNotaKomisiTambahanQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var entity = await dbContext.NotaKomisiTambahan.FindAsync(
                request.jns_sb_nt, request.kd_cb, request.jns_tr,
                request.jns_nt_msk, request.kd_thn, request.kd_bln, request.no_nt_msk,
                request.jns_nt_kel, request.no_nt_kel);
                                         
            if (entity == null)
                throw new NullReferenceException();

            return _mapper.Map<NotaKomisiTambahanDto>(entity);
        }
    }
}