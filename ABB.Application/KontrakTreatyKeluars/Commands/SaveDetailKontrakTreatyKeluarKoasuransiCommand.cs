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
    public class SaveDetailKontrakTreatyKeluarKoasuransiCommand : IRequest, IMapFrom<DetailKontrakTreatyKeluarKoasuransi>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public int no_urut { get; set; }

        public decimal pst_share_mul { get; set; }

        public decimal pst_share_akh { get; set; }
        
        public decimal pst_bts_koas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailKontrakTreatyKeluarKoasuransiCommand, DetailKontrakTreatyKeluarKoasuransi>();
        }
    }

    public class SaveDetailKontrakTreatyKeluarKoasuransiCommandHandler : IRequestHandler<SaveDetailKontrakTreatyKeluarKoasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDetailKontrakTreatyKeluarKoasuransiCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveDetailKontrakTreatyKeluarKoasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDetailKontrakTreatyKeluarKoasuransiCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveDetailKontrakTreatyKeluarKoasuransiCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarKoasuransi =
                    _contextPst.DetailKontrakTreatyKeluarKoasuransi.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.no_urut);
                
                if (detailKontrakTreatyKeluarKoasuransi == null)
                {
                    var detailKontrakTreatyKeluarKoasuransis = _contextPst.DetailKontrakTreatyKeluarKoasuransi.Where(m =>
                        m.kd_cb == request.kd_cb && m.kd_jns_sor == request.kd_jns_sor &&
                        m.kd_tty_pps == request.kd_tty_pps);

                    var no_urut = detailKontrakTreatyKeluarKoasuransis.Any()
                        ? detailKontrakTreatyKeluarKoasuransis.Select(s => s.no_urut).Max() + 1
                        : 1;

                    var newDetailKontrakTreatyKeluarKoasuransi = _mapper.Map<DetailKontrakTreatyKeluarKoasuransi>(request);
                    newDetailKontrakTreatyKeluarKoasuransi.no_urut = no_urut;
                    _contextPst.DetailKontrakTreatyKeluarKoasuransi.Add(newDetailKontrakTreatyKeluarKoasuransi);
                }
                else
                {
                    detailKontrakTreatyKeluarKoasuransi.pst_bts_koas = request.pst_bts_koas;
                    detailKontrakTreatyKeluarKoasuransi.pst_share_akh = request.pst_share_akh;
                    detailKontrakTreatyKeluarKoasuransi.pst_share_mul = request.pst_share_mul;
                }
                
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}