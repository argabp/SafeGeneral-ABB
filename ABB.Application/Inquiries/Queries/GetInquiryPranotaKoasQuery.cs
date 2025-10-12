using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryPranotaKoasQuery : IRequest<InquiryPranotaKoasDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }
    }

    public class GetInquiryPranotaKoasQueryHandler : IRequestHandler<GetInquiryPranotaKoasQuery, InquiryPranotaKoasDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryPranotaKoasQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryPranotaKoasDto> Handle(GetInquiryPranotaKoasQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var InquiryResiko = await dbContext.InquiryPranotaKoas.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                request.kd_mtu, request.kd_grp_pas, request.kd_rk_pas);
                                         
            if (InquiryResiko == null)
                throw new NullReferenceException();

            return _mapper.Map<InquiryPranotaKoasDto>(InquiryResiko);
        }
    }
}