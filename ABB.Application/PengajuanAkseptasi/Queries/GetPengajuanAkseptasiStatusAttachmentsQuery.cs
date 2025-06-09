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

        public string DatabaseName { get; set; }
    }

    public class GetPengajuanAkseptasiStatusAttachmentsQueryHandler : IRequestHandler<GetPengajuanAkseptasiStatusAttachmentsQuery, List<PengajuanAkseptasiStatusAttachmentDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetPengajuanAkseptasiStatusAttachmentsQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PengajuanAkseptasiStatusAttachmentDto>> Handle(GetPengajuanAkseptasiStatusAttachmentsQuery request,
            CancellationToken cancellationToken)
        {
            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            var result =
                (await _dbConnectionFactory.Query<PengajuanAkseptasiStatusAttachmentDto>(@"
                                                                                                Select p.*
                                                                                                    From TR_AkseptasiStatusAttachment p 
                                                                                                Where @kd_cb = p.kd_cb AND @kd_cob = p.kd_cob
                                                                                                AND @kd_scob = p.kd_scob AND @kd_thn = p.kd_thn
                                                                                                AND @no_aks = p.no_aks AND @no_urut = p.no_urut",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob, 
                        request.kd_thn, request.no_aks, request.no_urut
                    })).ToList();

            return result;
        }
    }
}