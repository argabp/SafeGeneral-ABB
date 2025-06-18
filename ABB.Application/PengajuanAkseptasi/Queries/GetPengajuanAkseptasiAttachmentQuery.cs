using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasiAttachmentQuery : IRequest<List<PengajuanAkseptasiAttachmentDto>>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
    }

    public class GetPengajuanAkseptasiAttachmentQueryHandler : IRequestHandler<GetPengajuanAkseptasiAttachmentQuery, List<PengajuanAkseptasiAttachmentDto>>
    {
        private readonly IDbConnection _db;

        public GetPengajuanAkseptasiAttachmentQueryHandler(IDbConnection db, ILogger<GetPengajuanAkseptasiAttachmentQueryHandler> logger
            ,IHostEnvironment host)
        {
            _db = db;
        }

        public async Task<List<PengajuanAkseptasiAttachmentDto>> Handle(GetPengajuanAkseptasiAttachmentQuery request,
            CancellationToken cancellationToken)
        {
            var lampirans = (await _db.Query<PengajuanAkseptasiAttachmentDto>(
                @$"SELECT a.*, d.nm_dokumen dokumenName FROM TR_AkseptasiAttachment a
                                INNER JOIN MS_DokumenDetil d
                                    ON a.kd_dokumen = d.kd_dokumen
                                   WHERE a.kd_cb = '{request.kd_cb}' AND 
                               a.kd_cob = '{request.kd_cob}' AND a.kd_scob = '{request.kd_scob}' AND a.kd_thn = '{request.kd_thn}' AND
                               a.no_aks = '{request.no_aks}'")).ToList();

            return lampirans;
        } 
    }
}