using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetRekananSorQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
        public string jns_lookup { get; set; }
        public string kd_cb { get; set; }
        public string kd_jns_sor { get; set; }
    }

    public class GetRekananSorQueryHandler : IRequestHandler<GetRekananSorQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetRekananSorQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRekananSorQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<string>("spg_ddlb_all",
                new
                {
                    request.jns_lookup, nm_kolom = "kd_rk_sor"
                })).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<DropdownOptionDto>();
            }

            var data = result?.Split("#")[1];
            
            var menuName = data?.Split(",")[0];
            var dbName = data?.Split(",")[1];

            List<DropdownOptionDto> dropdown;
            
            if (menuName.Contains("Rekanan"))
            {
                dropdown = (await _connectionFactory.Query<DropdownOptionDto>($"SELECT kd_rk Value, nm_rk Text FROM {dbName} WHERE kd_grp_rk = '5' AND kd_cb = '{request.kd_cb}'")).ToList();
            }
            else
            {
                dropdown = (await _connectionFactory.Query<DropdownOptionDto>($"SELECT kd_tty_pps Value, nm_tty_pps Text FROM {dbName} WHERE kd_jns_sor = '{request.kd_jns_sor}'")).ToList();
            }

            return dropdown;
        }
    }
}