using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.CancelCSM.Queries
{
    public class GetViewSourceDataCancelQuery : IRequest<List<ViewSourceDataCancel>>
    {
        public string TipeTransaksi { get; set; }

        public string KodeMetode { get; set; }
    }

    public class GetViewSourceDataCancelQueryHandler : IRequestHandler<GetViewSourceDataCancelQuery, List<ViewSourceDataCancel>>
    {
        private readonly IDbContextCSM _dbContextCsm;

        public GetViewSourceDataCancelQueryHandler(IDbContextCSM dbContextCsm)
        {
            _dbContextCsm = dbContextCsm;
        }

        public async Task<List<ViewSourceDataCancel>> Handle(GetViewSourceDataCancelQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            List<ViewSourceDataCancel> viewSourceDatas;
                
            if(string.IsNullOrWhiteSpace(request.KodeMetode) && string.IsNullOrWhiteSpace(request.TipeTransaksi))
                viewSourceDatas = 
                    _dbContextCsm.ViewSourceDataCancel
                        .Where(w => w.KodeMetode == request.KodeMetode && w.TipeTransaksi == request.TipeTransaksi).ToList();
            else if(string.IsNullOrWhiteSpace(request.KodeMetode))
                viewSourceDatas = _dbContextCsm.ViewSourceDataCancel
                    .Where(w => w.TipeTransaksi == request.TipeTransaksi).ToList();
            else if(string.IsNullOrWhiteSpace(request.TipeTransaksi))
                viewSourceDatas = _dbContextCsm.ViewSourceDataCancel
                    .Where(w => w.KodeMetode == request.KodeMetode).ToList();
            else
                viewSourceDatas = _dbContextCsm.ViewSourceDataCancel
                    .Where(w => w.KodeMetode == request.KodeMetode && w.TipeTransaksi == request.TipeTransaksi).ToList();

            return viewSourceDatas;
        }
    }
}