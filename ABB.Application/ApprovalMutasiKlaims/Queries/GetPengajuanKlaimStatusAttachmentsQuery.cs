using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ApprovalMutasiKlaims.Queries
{
    public class GetPengajuanKlaimStatusAttachmentsQuery : IRequest<List<PengajuanKlaimStatusAttachmentDto>>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
        
        public Int16 no_urut { get; set; }

        public string DatabaseName { get; set; }
    }

    public class GetPengajuanKlaimStatusAttachmentsQueryHandler : IRequestHandler<GetPengajuanKlaimStatusAttachmentsQuery, List<PengajuanKlaimStatusAttachmentDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetPengajuanKlaimStatusAttachmentsQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PengajuanKlaimStatusAttachmentDto>> Handle(GetPengajuanKlaimStatusAttachmentsQuery request,
            CancellationToken cancellationToken)
        {
            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            var result =
                (await _dbConnectionFactory.Query<PengajuanKlaimStatusAttachmentDto>(@"
                                                                                                Select p.*
                                                                                                    From TR_KlaimStatusAttachment p 
                                                                                                Where @kd_cb = p.kd_cb AND @kd_cob = p.kd_cob
                                                                                                AND @kd_scob = p.kd_scob AND @kd_thn = p.kd_thn
                                                                                                AND @no_kl = p.no_kl AND @no_mts = p.no_mts 
                                                                                                AND @no_urut = p.no_urut",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob, 
                        request.kd_thn, request.no_kl, request.no_mts,
                        request.no_urut
                    })).ToList();

            return result;
        }
    }
}