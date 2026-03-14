using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasuks.Commands
{
    public class SaveKontrakTreatyMasukCommand : IRequest, IMapFrom<KontrakTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string tipe_tty_msk { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_uw { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? desk_tty { get; set; }

        public decimal pst_share_tty { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveKontrakTreatyMasukCommand, KontrakTreatyMasuk>();
        }
    }

    public class SaveKontrakTreatyMasukCommandHandler : IRequestHandler<SaveKontrakTreatyMasukCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<SaveKontrakTreatyMasukCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveKontrakTreatyMasukCommandHandler(IDbContextPst contextPst, IDbConnectionPst connectionPst,
            ILogger<SaveKontrakTreatyMasukCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _connectionPst = connectionPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveKontrakTreatyMasukCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyMasuk =
                    _contextPst.KontrakTreatyMasuk.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_msk);

                if (kontrakTreatyMasuk == null)
                {
                    var kd_tty_msk = (await _connectionPst.QueryProc<string>("spe_ri01e_01", new
                    {
                        request.kd_cb, request.kd_jns_sor, request.thn_uw
                    })).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(kd_tty_msk))
                    {
                        _logger.LogInformation("Null Result exec SP spe_ri01e_01 '{kd_cb}', '{kd_jns_sor}', '{thn_uw}'",
                            request.kd_cb, request.kd_jns_sor, request.thn_uw);
                        throw new NullReferenceException(kd_tty_msk);
                    }
                    
                    var newKontrakTreatyMasuk = _mapper.Map<KontrakTreatyMasuk>(request);
                    newKontrakTreatyMasuk.kd_tty_msk = kd_tty_msk.Split(",")[1];
                    _contextPst.KontrakTreatyMasuk.Add(newKontrakTreatyMasuk);
                }
                else
                {
                    kontrakTreatyMasuk.tipe_tty_msk = request.tipe_tty_msk;
                    kontrakTreatyMasuk.kd_cob = request.kd_cob;
                    kontrakTreatyMasuk.thn_uw = request.thn_uw;
                    kontrakTreatyMasuk.kd_grp_pas = request.kd_grp_pas;
                    kontrakTreatyMasuk.kd_rk_pas = request.kd_rk_pas;
                    kontrakTreatyMasuk.kd_grp_sb_bis = request.kd_grp_sb_bis;
                    kontrakTreatyMasuk.kd_rk_sb_bis = request.kd_rk_sb_bis;
                    kontrakTreatyMasuk.desk_tty = request.desk_tty;
                    kontrakTreatyMasuk.pst_share_tty = request.pst_share_tty;
                    kontrakTreatyMasuk.tgl_mul_ptg = request.tgl_mul_ptg;
                    kontrakTreatyMasuk.tgl_akh_ptg = request.tgl_akh_ptg;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}