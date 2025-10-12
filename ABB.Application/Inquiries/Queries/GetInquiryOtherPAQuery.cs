using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryOtherPAQuery : IRequest<InquiryOtherPADto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public int no_updt { get; set; }

        public int no_rsk { get; set; }

        public string kd_endt { get; set; }
    }

    public class GetInquiryOtherPAQueryHandler : IRequestHandler<GetInquiryOtherPAQuery, InquiryOtherPADto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryOtherPAQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryOtherPADto> Handle(GetInquiryOtherPAQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var InquiryResiko = await dbContext.InquiryOtherPA.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                request.no_rsk, request.kd_endt);

            return InquiryResiko == null ? null : _mapper.Map<InquiryOtherPADto>(InquiryResiko);
        }
    }
}