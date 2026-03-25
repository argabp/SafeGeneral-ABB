using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluarXOLs.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Commands
{
    public class SaveKontrakTreatyKeluarXOLCommand : IRequest, IMapFrom<KontrakTreatyKeluarXOL>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_tty_npps { get; set; }

        public string npps_layer { get; set; }
        
        public string nm_tty_npps { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public decimal nilai_bts_or { get; set; }

        public decimal nilai_bts_tty { get; set; }

        public decimal pst_adj_onrpi { get; set; }

        public string? ket_tty_npps { get; set; }

        public decimal? nilai_kurs { get; set; }

        public decimal? pst_reinst { get; set; }

        public decimal? mindep { get; set; }

        public short? hit { get; set; }

        public List<DetailKontrakTreatyKeluarXOLDataDto> Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveKontrakTreatyKeluarXOLCommand, KontrakTreatyKeluarXOL>();
        }
    }

    public class SaveKontrakTreatyKeluarXOLCommandHandler : IRequestHandler<SaveKontrakTreatyKeluarXOLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<SaveKontrakTreatyKeluarXOLCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveKontrakTreatyKeluarXOLCommandHandler(IDbContextPst contextPst, IDbConnectionPst connectionPst,
            ILogger<SaveKontrakTreatyKeluarXOLCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _connectionPst = connectionPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveKontrakTreatyKeluarXOLCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyKeluarXOL =
                    _contextPst.KontrakTreatyKeluarXOL.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_npps);

                var kd_tty_npps = request.kd_tty_npps;
                
                if (kontrakTreatyKeluarXOL == null)
                {
                    kd_tty_npps = (await _connectionPst.QueryProc<string>("spe_ri04e_01", new
                    {
                        request.kd_cb, request.kd_jns_sor, thn_tty_pps = request.thn_tty_npps
                    })).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(kd_tty_npps))
                    {
                        _logger.LogInformation("Null Result exec SP spe_ri01e_01 '{kd_cb}', '{kd_jns_sor}', '{thn_uw}'",
                            request.kd_cb, request.kd_jns_sor, request.thn_tty_npps);
                        throw new NullReferenceException(kd_tty_npps);
                    }

                    kd_tty_npps = kd_tty_npps.Split(",")[1];
                    
                    var newKontrakTreatyKeluarXOL = _mapper.Map<KontrakTreatyKeluarXOL>(request);
                    newKontrakTreatyKeluarXOL.kd_tty_npps = kd_tty_npps;
                    _contextPst.KontrakTreatyKeluarXOL.Add(newKontrakTreatyKeluarXOL);
                }
                else
                {
                    kontrakTreatyKeluarXOL.kd_cob = request.kd_cob;
                    kontrakTreatyKeluarXOL.npps_layer = request.npps_layer;
                    kontrakTreatyKeluarXOL.nm_tty_npps = request.nm_tty_npps;
                    kontrakTreatyKeluarXOL.thn_tty_npps = request.thn_tty_npps;
                    kontrakTreatyKeluarXOL.tgl_mul_ptg = request.tgl_mul_ptg;
                    kontrakTreatyKeluarXOL.tgl_akh_ptg = request.tgl_akh_ptg;
                    kontrakTreatyKeluarXOL.nilai_bts_or = request.nilai_bts_or;
                    kontrakTreatyKeluarXOL.nilai_bts_tty = request.nilai_bts_or;
                    kontrakTreatyKeluarXOL.pst_adj_onrpi = request.pst_adj_onrpi;
                    kontrakTreatyKeluarXOL.ket_tty_npps = request.ket_tty_npps;
                    kontrakTreatyKeluarXOL.nilai_kurs = request.nilai_kurs;
                    kontrakTreatyKeluarXOL.pst_reinst = request.pst_reinst;
                }
                
                var detailKontrakTreatyKeluarXOLs =
                    _contextPst.DetailKontrakTreatyKeluarXOL.Where(w => w.kd_cb == request.kd_cb 
                                                                        && w.kd_jns_sor == request.kd_jns_sor
                                                                        && w.kd_tty_npps == request.kd_tty_npps);
                
                _contextPst.DetailKontrakTreatyKeluarXOL.RemoveRange(detailKontrakTreatyKeluarXOLs);

                foreach (var detail in request.Details)
                {
                    var detailKontrakTreatyKeluarXOL = _mapper.Map<DetailKontrakTreatyKeluarXOL>(detail);
                    detailKontrakTreatyKeluarXOL.kd_tty_npps = kd_tty_npps;
                    detailKontrakTreatyKeluarXOL.kd_cb = request.kd_cb;
                    detailKontrakTreatyKeluarXOL.kd_jns_sor = request.kd_jns_sor;
                    
                    _contextPst.DetailKontrakTreatyKeluarXOL.Add(detailKontrakTreatyKeluarXOL);
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}