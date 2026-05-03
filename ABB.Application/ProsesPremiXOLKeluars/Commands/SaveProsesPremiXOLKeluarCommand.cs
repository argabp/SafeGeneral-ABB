using System;
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

namespace ABB.Application.ProsesPremiXOLKeluars.Commands
{
    public class SaveProsesPremiXOLKeluarCommand : IRequest, IMapFrom<ProsesPremiXOLKeluar>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }

        public string? ket_tr { get; set; }

        public string no_ref { get; set; }

        public decimal nilai_prm { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveProsesPremiXOLKeluarCommand, ProsesPremiXOLKeluar>();
        }
    }

    public class SaveProsesPremiXOLKeluarCommandHandler : IRequestHandler<SaveProsesPremiXOLKeluarCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<SaveProsesPremiXOLKeluarCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SaveProsesPremiXOLKeluarCommandHandler(IDbContextPst contextPst, IDbConnectionPst connectionPst,
            ILogger<SaveProsesPremiXOLKeluarCommandHandler> logger, IMapper mapper, ICurrentUserService currentUserService)
        {;
            _contextPst = contextPst;
            _connectionPst = connectionPst;
            _logger = logger;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(SaveProsesPremiXOLKeluarCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var prosesPremiXOLKeluar =
                    _contextPst.ProsesPremiXOLKeluar.Find(request.kd_cb, request.kd_thn, request.kd_bln, request.kd_jns_sor,
                        request.kd_tty_npps, request.kd_mtu, request.no_tr);

                var no_tr = request.no_tr;
                
                if (prosesPremiXOLKeluar == null)
                {
                    request.kd_bln = request.tgl_closing.ToString("MM");
                    request.kd_thn = request.tgl_closing.ToString("yy");
                    
                    no_tr = (await _connectionPst.QueryProc<string>("spe_ri09e_01", new
                    {
                        request.kd_cb, request.kd_jns_sor, kd_tty_msk = request.kd_tty_npps,
                        request.kd_thn, request.kd_bln, request.kd_mtu
                    })).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(no_tr))
                    {
                        _logger.LogInformation("Null Result exec SP spe_ri09e_01 '{kd_cb}', '{kd_jns_sor}', '{kd_tty_msk}', '{kd_thn}', '{kd_bln}', '{kd_mtu}'",
                            request.kd_cb, request.kd_jns_sor, request.kd_tty_npps, request.kd_thn, request.kd_bln, request.kd_mtu);
                        throw new NullReferenceException(no_tr);
                    }

                    no_tr = no_tr.Split(",")[1];
                    
                    var newProsesPremiXOLKeluar = _mapper.Map<ProsesPremiXOLKeluar>(request);
                    newProsesPremiXOLKeluar.no_tr = no_tr;
                    newProsesPremiXOLKeluar.kd_usr_input = _currentUserService.UserId;
                    newProsesPremiXOLKeluar.tgl_input = DateTime.Now;
                    newProsesPremiXOLKeluar.flag_closing = "N";
                    _contextPst.ProsesPremiXOLKeluar.Add(newProsesPremiXOLKeluar);
                }
                else
                {
                    prosesPremiXOLKeluar.tgl_closing = request.tgl_closing;
                    prosesPremiXOLKeluar.no_ref = request.no_ref;
                    prosesPremiXOLKeluar.ket_tr = request.ket_tr;
                    prosesPremiXOLKeluar.nilai_prm = request.nilai_prm;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}