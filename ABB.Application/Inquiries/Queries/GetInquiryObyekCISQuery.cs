using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryObyekCISQuery : IRequest<InquiryObyekCISDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }
    }

    public class GetInquiryObyekCISQueryHandler : IRequestHandler<GetInquiryObyekCISQuery, InquiryObyekCISDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryObyekCISQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryObyekCISDto> Handle(GetInquiryObyekCISQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var InquiryResiko = await dbContext.InquiryObyekCIS.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                request.no_rsk, request.kd_endt, request.no_oby);
                                         
            if (InquiryResiko == null)
                throw new NullReferenceException();

            return _mapper.Map<InquiryObyekCISDto>(InquiryResiko);
        }
    }
}