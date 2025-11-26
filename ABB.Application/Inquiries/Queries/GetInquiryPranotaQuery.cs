using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Inquiries.Queries
{
    public class GetInquiryPranotaQuery : IRequest<InquiryPranotaDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetInquiryPranotaQueryHandler : IRequestHandler<GetInquiryPranotaQuery, InquiryPranotaDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetInquiryPranotaQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<InquiryPranotaDto> Handle(GetInquiryPranotaQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
            var InquiryResiko = dbContext.InquiryPranota.FirstOrDefault(w => 
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn &&
                    w.no_pol == request.no_pol && w.no_updt == request.no_updt);

            return InquiryResiko == null ? null : _mapper.Map<InquiryPranotaDto>(InquiryResiko);
        }
    }
}