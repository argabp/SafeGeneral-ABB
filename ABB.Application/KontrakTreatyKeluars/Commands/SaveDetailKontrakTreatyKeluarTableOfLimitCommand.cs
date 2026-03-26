using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Commands
{
    public class SaveDetailKontrakTreatyKeluarTableOfLimitCommand : IRequest, IMapFrom<DetailKontrakTreatyKeluarTableOfLimit>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string category_rsk { get; set; }

        public string kd_kls_konstr { get; set; }
        
        public decimal pst_bts_tty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailKontrakTreatyKeluarTableOfLimitCommand, DetailKontrakTreatyKeluarTableOfLimit>();
        }
    }

    public class SaveDetailKontrakTreatyKeluarTableOfLimitCommandHandler : IRequestHandler<SaveDetailKontrakTreatyKeluarTableOfLimitCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDetailKontrakTreatyKeluarTableOfLimitCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveDetailKontrakTreatyKeluarTableOfLimitCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDetailKontrakTreatyKeluarTableOfLimitCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveDetailKontrakTreatyKeluarTableOfLimitCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarTableOfLimit =
                    _contextPst.DetailKontrakTreatyKeluarTableOfLimit.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_okup, request.category_rsk, request.kd_kls_konstr);
                
                if (detailKontrakTreatyKeluarTableOfLimit == null)
                {
                    var newDetailKontrakTreatyKeluarTableOfLimit = _mapper.Map<DetailKontrakTreatyKeluarTableOfLimit>(request);
                    _contextPst.DetailKontrakTreatyKeluarTableOfLimit.Add(newDetailKontrakTreatyKeluarTableOfLimit);
                }
                else
                {
                    detailKontrakTreatyKeluarTableOfLimit.pst_bts_tty = request.pst_bts_tty;
                }
                
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}