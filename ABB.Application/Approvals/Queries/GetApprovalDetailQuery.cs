using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Approvals.Queries
{
    public class GetApprovalDetailQuery : IRequest<ApprovalDetailDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user { get; set; }

        public string kd_user_sign { get; set; }
    }

    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailQuery, ApprovalDetailDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetApprovalDetailQueryHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ApprovalDetailDto> Handle(GetApprovalDetailQuery request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var approval = dbContext.ApprovalDetail.FirstOrDefault(approval => approval.kd_cb.Trim() == request.kd_cb.Trim()
                                                                         && approval.kd_cob.Trim() == request.kd_cob.Trim()
                                                                         && approval.kd_scob.Trim() == request.kd_scob.Trim()
                                                                         && approval.kd_status == request.kd_status
                                                                         && approval.kd_user == request.kd_user
                                                                         && approval.kd_user_sign == request.kd_user_sign);

            if (approval == null)
                throw new NullReferenceException();

            return _mapper.Map<ApprovalDetailDto>(approval);
        }
    }
}