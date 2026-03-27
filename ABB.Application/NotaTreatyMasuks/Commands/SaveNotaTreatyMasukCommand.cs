using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasuks.Commands
{
    public class SaveNotaTreatyMasukCommand : IRequest, IMapFrom<TransaksiTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }

        public decimal thn_tr { get; set; }

        public byte? kuartal_tr { get; set; }

        public string? ket_tr { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal nilai_kms { get; set; }

        public decimal nilai_kl { get; set; }

        public string? nm_ttg { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime tgl_input { get; set; }

        public string? kd_usr_updt { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_closing { get; set; }

        public string flag_closing { get; set; }

        public byte? wpc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveNotaTreatyMasukCommand, NotaTreatyMasuk>();
        }
    }

    public class SaveNotaTreatyMasukCommandHandler : IRequestHandler<SaveNotaTreatyMasukCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<SaveNotaTreatyMasukCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SaveNotaTreatyMasukCommandHandler(IDbContextPst contextPst, IDbConnectionPst connectionPst,
            ILogger<SaveNotaTreatyMasukCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService)
        {;
            _contextPst = contextPst;
            _connectionPst = connectionPst;
            _logger = logger;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(SaveNotaTreatyMasukCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var transaksiTreatyMasuk =
                    _contextPst.TransaksiTreatyMasuk.Find(request.kd_cb, request.kd_thn, request.kd_bln, request.kd_jns_sor,
                        request.kd_tty_msk, request.kd_mtu, request.no_tr);

                var no_tr = request.no_tr;
                
                if (transaksiTreatyMasuk == null)
                {
                    request.kd_bln = request.tgl_closing.ToString("MM");
                    request.kd_thn = request.tgl_closing.ToString("yy");
                    
                    no_tr = (await _connectionPst.QueryProc<string>("spe_ri02e_01", new
                    {
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_msk,
                        request.kd_thn, request.kd_bln, request.kd_mtu
                    })).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(no_tr))
                    {
                        _logger.LogInformation("Null Result exec SP spe_ri02e_01 '{kd_cb}', '{kd_jns_sor}', '{kd_tty_msk}', '{kd_thn}', '{kd_bln}', '{kd_mtu}'",
                            request.kd_cb, request.kd_jns_sor, request.kd_tty_msk, request.kd_thn, request.kd_bln, request.kd_mtu);
                        throw new NullReferenceException(no_tr);
                    }

                    no_tr = no_tr.Split(",")[1];
                    
                    var newTransaksiTreatyMasuk = _mapper.Map<TransaksiTreatyMasuk>(request);
                    newTransaksiTreatyMasuk.no_tr = no_tr;
                    newTransaksiTreatyMasuk.kd_usr_input = _currentUserService.UserId;
                    newTransaksiTreatyMasuk.tgl_input = DateTime.Now;
                    newTransaksiTreatyMasuk.flag_closing = "N";
                    _contextPst.TransaksiTreatyMasuk.Add(newTransaksiTreatyMasuk);
                }
                else
                {
                    transaksiTreatyMasuk.tgl_closing = request.tgl_closing;
                    transaksiTreatyMasuk.kuartal_tr = request.kuartal_tr;
                    transaksiTreatyMasuk.thn_tr = request.thn_tr;
                    transaksiTreatyMasuk.nm_ttg = request.nm_ttg;
                    transaksiTreatyMasuk.ket_tr = request.ket_tr;
                    transaksiTreatyMasuk.nilai_ttl_ptg = request.nilai_ttl_ptg;
                    transaksiTreatyMasuk.nilai_prm = request.nilai_prm;
                    transaksiTreatyMasuk.nilai_kms = request.nilai_kms;
                    transaksiTreatyMasuk.nilai_kl = request.nilai_kl;
                    transaksiTreatyMasuk.nm_ttg = request.nm_ttg;
                    transaksiTreatyMasuk.wpc = request.wpc;
                    transaksiTreatyMasuk.kd_usr_updt = _currentUserService.UserId;
                    transaksiTreatyMasuk.tgl_updt = DateTime.Now;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}