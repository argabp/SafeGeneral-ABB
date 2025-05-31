using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Approvals.Queries
{
    public class GetApprovalQuery : IRequest<ApprovalDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetApprovalQueryHandler : IRequestHandler<GetApprovalQuery, ApprovalDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetApprovalQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ApprovalDto> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var approval = dbContext.Approval.FirstOrDefault(approval => approval.kd_cb.Trim() == request.kd_cb.Trim()
                                                            && approval.kd_cob.Trim() == request.kd_cob.Trim()
                                                            && approval.kd_scob.Trim() == request.kd_scob.Trim());

            if (approval == null)
                throw new NullReferenceException();

            return _mapper.Map<ApprovalDto>(approval);
        }
    }
}