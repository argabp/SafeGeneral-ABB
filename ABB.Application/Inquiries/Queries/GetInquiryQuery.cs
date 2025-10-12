using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryQuery : IRequest<InquiryDto?>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetInquiryQueryHandler : IRequestHandler<GetInquiryQuery, InquiryDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryDto?> Handle(GetInquiryQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var Inquiry = await dbContext.Inquiry.FindAsync(request.kd_cb, 
                                             request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt);

            return Inquiry == null ? null : _mapper.Map<InquiryDto>(Inquiry);
        }
    }
}