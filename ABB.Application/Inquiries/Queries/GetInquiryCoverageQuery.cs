using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryCoverageQuery : IRequest<InquiryCoverageDto>
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

        public string kd_cvrg { get; set; }
    }

    public class GetInquiryCoverageQueryHandler : IRequestHandler<GetInquiryCoverageQuery, InquiryCoverageDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryCoverageQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryCoverageDto> Handle(GetInquiryCoverageQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var InquiryResiko = await dbContext.InquiryCoverage.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                request.no_rsk, request.kd_endt, request.kd_cvrg);
                                         
            if (InquiryResiko == null)
                throw new NullReferenceException();

            return _mapper.Map<InquiryCoverageDto>(InquiryResiko);
        }
    }
}