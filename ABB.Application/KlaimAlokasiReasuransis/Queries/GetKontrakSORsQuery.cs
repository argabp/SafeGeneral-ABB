using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetKontrakSORsQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
    }

    public class GetKontrakSORsQueryHandler : IRequestHandler<GetKontrakSORsQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;

        public GetKontrakSORsQueryHandler(IDbConnectionPst connectionPst)
        {
            _connectionPst = connectionPst;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKontrakSORsQuery request, CancellationToken cancellationToken)
        {
            return (await _connectionPst.Query<DropdownOptionDto>("Select kd_tty_npps Value, nm_tty_npps Text From v_ri02t WHERE kd_cob = @kd_cob", new
            {
                request.kd_cob
            })).ToList();
        }
    }
}