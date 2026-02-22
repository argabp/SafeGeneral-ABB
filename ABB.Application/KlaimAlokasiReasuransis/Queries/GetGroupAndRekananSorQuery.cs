using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetGroupAndRekananSorQuery : IRequest<string>
    {
        public string kd_jns_sor { get; set; }
    }

    public class GetGroupAndRekananSorQueryHandler : IRequestHandler<GetGroupAndRekananSorQuery, string>
    {
        private readonly IDbConnectionPst _connectionPst;

        public GetGroupAndRekananSorQueryHandler(IDbConnectionPst connectionPst)
        {
            _connectionPst = connectionPst;
        }

        public async Task<string> Handle(GetGroupAndRekananSorQuery request, CancellationToken cancellationToken)
        {
            return (await _connectionPst.QueryProc<string>("spe_cl01e_02",
                new
                {
                    request.kd_jns_sor
                })).FirstOrDefault();
        }
    }
}