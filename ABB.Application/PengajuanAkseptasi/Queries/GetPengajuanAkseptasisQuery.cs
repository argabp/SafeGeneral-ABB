using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasisQuery : IRequest<List<PengajuanAkseptasiDto>>
    {
        public string SearchKeyword { get; set; }

        public string DatabaseName { get; set; }
    }

    public class GetPengajuanAkseptasisQueryHandler : IRequestHandler<GetPengajuanAkseptasisQuery, List<PengajuanAkseptasiDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ICurrentUserService _userService;

        public GetPengajuanAkseptasisQueryHandler(IDbConnectionFactory dbConnectionFactory, ICurrentUserService userService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _userService = userService;
        }

        public async Task<List<PengajuanAkseptasiDto>> Handle(GetPengajuanAkseptasisQuery request,
            CancellationToken cancellationToken)
        {
            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            var results =
                (await _dbConnectionFactory.Query<PengajuanAkseptasiDto>(@"SELECT * FROM v_TR_Akseptasi Where (nm_cb like '%'+@SearchKeyword+'%'
			                                                                        OR nm_cob like '%'+@SearchKeyword+'%'
			                                                                        OR nm_scob like '%'+@SearchKeyword+'%'
			                                                                        OR nm_tertanggung like '%'+@SearchKeyword+'%'
			                                                                        OR nomor_pengajuan like '%'+@SearchKeyword+'%'
			                                                                        OR tgl_status like '%'+@SearchKeyword+'%'
			                                                                        OR status like '%'+@SearchKeyword+'%'
			                                                                        OR user_status like '%'+@SearchKeyword+'%'
			                                                                        OR tgl_pengajuan like '%'+@SearchKeyword+'%'
			                                                                        OR @SearchKeyword = '' OR @SearchKeyword is null
		                                                                        )", new { _userService.UserId,  request.SearchKeyword })).ToList();

            foreach (var result in results)
            {
                result.Id =
                    $"{result.kd_cb.Trim()}{result.kd_cob.Trim()}{result.kd_scob.Trim()}{result.kd_thn}{result.no_aks}";
            }
            
            return results.ToList();
        }
    }
}