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
    public class SaveKontrakTreatyKeluarCommand : IRequest<KontrakTreatyKeluar>, IMapFrom<KontrakTreatyKeluar>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_tty_pps { get; set; }
        
        public string nm_tty_pps { get; set; }
        
        public string? nm_jns_ptg { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public string frek_lap { get; set; }

        public decimal nilai_bts_cash_call { get; set; }

        public decimal nilai_bts_cash_call_idr { get; set; }

        public decimal nilai_bts_or { get; set; }
        
        public decimal nilai_bts_or_idr { get; set; }

        public decimal nilai_bts_tty { get; set; }

        public decimal nilai_bts_tty_idr { get; set; }

        public decimal pst_kms_reas { get; set; }

        public string? ket_tty_pps { get; set; }

        public decimal? pst_share_reas { get; set; }

        public decimal? pst_hf { get; set; }

        public decimal? nilai_kurs { get; set; }

        public decimal nilai_bts_cash_pla { get; set; }

        public decimal nilai_bts_cash_pla_idr { get; set; }

        public decimal? pst_profit_comm { get; set; }

        public string? faktor_sor { get; set; }
        
        public string? st_koas { get; set; }

        public List<DetailKontrakTreatyKeluarDataDto> Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveKontrakTreatyKeluarCommand, KontrakTreatyKeluar>();
        }
    }

    public class SaveKontrakTreatyKeluarCommandHandler : IRequestHandler<SaveKontrakTreatyKeluarCommand, KontrakTreatyKeluar>
    {
        private readonly IDbContextPst _contextPst;
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<SaveKontrakTreatyKeluarCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveKontrakTreatyKeluarCommandHandler(IDbContextPst contextPst, IDbConnectionPst connectionPst,
            ILogger<SaveKontrakTreatyKeluarCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _connectionPst = connectionPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<KontrakTreatyKeluar> Handle(SaveKontrakTreatyKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyKeluar =
                    _contextPst.KontrakTreatyKeluar.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps);

                var kd_tty_pps = request.kd_tty_pps;
                
                if (kontrakTreatyKeluar == null)
                {
                    kd_tty_pps = (await _connectionPst.QueryProc<string>("spe_ri03e_01", new
                    {
                        request.kd_cb, request.kd_jns_sor, request.thn_tty_pps
                    })).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(kd_tty_pps))
                    {
                        _logger.LogInformation("Null Result exec SP spe_ri03e_01 '{kd_cb}', '{kd_jns_sor}', '{thn_uw}'",
                            request.kd_cb, request.kd_jns_sor, request.thn_tty_pps);
                        throw new NullReferenceException(kd_tty_pps);
                    }

                    kd_tty_pps = kd_tty_pps.Split(",")[1];
                    
                    kontrakTreatyKeluar = _mapper.Map<KontrakTreatyKeluar>(request);
                    kontrakTreatyKeluar.kd_tty_pps = kd_tty_pps;
                    _contextPst.KontrakTreatyKeluar.Add(kontrakTreatyKeluar);
                }
                else
                {
                    kontrakTreatyKeluar.kd_cob = request.kd_cob;
                    kontrakTreatyKeluar.thn_tty_pps = request.thn_tty_pps;
                    kontrakTreatyKeluar.nm_tty_pps = request.nm_tty_pps;
                    kontrakTreatyKeluar.tgl_mul_ptg = request.tgl_mul_ptg;
                    kontrakTreatyKeluar.tgl_akh_ptg = request.tgl_akh_ptg;
                    kontrakTreatyKeluar.frek_lap = request.frek_lap;
                    kontrakTreatyKeluar.nilai_bts_cash_call = request.nilai_bts_cash_call;
                    kontrakTreatyKeluar.nilai_bts_cash_call_idr = request.nilai_bts_cash_call_idr;
                    kontrakTreatyKeluar.nilai_bts_or = request.nilai_bts_or;
                    kontrakTreatyKeluar.nilai_bts_or_idr = request.nilai_bts_or_idr;
                    kontrakTreatyKeluar.nilai_bts_tty = request.nilai_bts_or;
                    kontrakTreatyKeluar.nilai_bts_tty_idr = request.nilai_bts_tty_idr;
                    kontrakTreatyKeluar.nilai_bts_cash_pla = request.nilai_bts_cash_pla;
                    kontrakTreatyKeluar.nilai_bts_cash_pla_idr = request.nilai_bts_cash_pla_idr;
                    kontrakTreatyKeluar.nilai_kurs = request.nilai_kurs;
                    kontrakTreatyKeluar.pst_share_reas = request.pst_share_reas;
                    kontrakTreatyKeluar.faktor_sor = request.faktor_sor;
                    kontrakTreatyKeluar.pst_kms_reas = request.pst_kms_reas;
                    kontrakTreatyKeluar.pst_hf = request.pst_hf;
                    kontrakTreatyKeluar.pst_profit_comm = request.pst_profit_comm;
                    kontrakTreatyKeluar.ket_tty_pps = request.ket_tty_pps;
                }
                
                var detailKontrakTreatyKeluars =
                    _contextPst.DetailKontrakTreatyKeluar.Where(w => w.kd_cb == request.kd_cb 
                                                                        && w.kd_jns_sor == request.kd_jns_sor
                                                                        && w.kd_tty_pps == request.kd_tty_pps);
                
                _contextPst.DetailKontrakTreatyKeluar.RemoveRange(detailKontrakTreatyKeluars);

                foreach (var detail in request.Details)
                {
                    var detailKontrakTreatyKeluar = _mapper.Map<DetailKontrakTreatyKeluar>(detail);
                    detailKontrakTreatyKeluar.kd_tty_pps = kd_tty_pps;
                    detailKontrakTreatyKeluar.kd_cb = request.kd_cb;
                    detailKontrakTreatyKeluar.kd_jns_sor = request.kd_jns_sor;
                    
                    _contextPst.DetailKontrakTreatyKeluar.Add(detailKontrakTreatyKeluar);
                }

                await _contextPst.SaveChangesAsync(cancellationToken);

                return kontrakTreatyKeluar;
            }, _logger);
        }
    }
}