using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.ProsesCSM.Queries
{
    public class GetViewSourceDataQuery : IRequest<List<ViewSourceData>>
    {
        // public string TipeTransaksi { get; set; }

        public string KodeMetode { get; set; }
    }

    public class GetViewSourceDataQueryHandler : IRequestHandler<GetViewSourceDataQuery, List<ViewSourceData>>
    {
        private readonly IDbContextCSM _dbContextCsm;

        public GetViewSourceDataQueryHandler(IDbContextCSM dbContextCsm)
        {
            _dbContextCsm = dbContextCsm;
        }

        public async Task<List<ViewSourceData>> Handle(GetViewSourceDataQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            List<ViewSourceData> viewSourceDatas;
                
            if(string.IsNullOrWhiteSpace(request.KodeMetode))
                viewSourceDatas = 
                    _dbContextCsm.ViewSourceData
                        .Where(w => w.KodeMetode == request.KodeMetode).ToList();
            else if(string.IsNullOrWhiteSpace(request.KodeMetode))
                viewSourceDatas = _dbContextCsm.ViewSourceData.ToList();
            else
                viewSourceDatas = _dbContextCsm.ViewSourceData
                .Where(w => w.KodeMetode == request.KodeMetode).ToList();

            return viewSourceDatas;
        }
    }
}