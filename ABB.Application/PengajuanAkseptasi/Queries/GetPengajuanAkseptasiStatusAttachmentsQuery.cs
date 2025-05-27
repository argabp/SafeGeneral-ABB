using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasiStatusAttachmentsQuery : IRequest<List<PengajuanAkseptasiStatusAttachmentDto>>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
        
        public Int16 no_urut { get; set; }
    }

    public class GetPengajuanAkseptasiStatusAttachmentsQueryHandler : IRequestHandler<GetPengajuanAkseptasiStatusAttachmentsQuery, List<PengajuanAkseptasiStatusAttachmentDto>>
    {
        private readonly IDbConnection _db;

        public GetPengajuanAkseptasiStatusAttachmentsQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<PengajuanAkseptasiStatusAttachmentDto>> Handle(GetPengajuanAkseptasiStatusAttachmentsQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                (await _db.QueryProc<PengajuanAkseptasiStatusAttachmentDto>("sp_PENGAJUANAKSEPTASI_GetPengajuanAkseptasiStatusAttachment",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob, 
                        request.kd_thn, request.no_aks, request.no_urut
                    })).ToList();

            return result;
        }
    }
}