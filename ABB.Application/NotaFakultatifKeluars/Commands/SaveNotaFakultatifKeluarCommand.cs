using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.NotaFakultatifKeluars.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaFakultatifKeluars.Commands
{
    public class SaveNotaFakultatifKeluarCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? ket_nt { get; set; }

        public string? ket_tc { get; set; }

        public List<DetailNotaFakultatifKeluarDto> Details { get; set; }
    }

    public class SaveNotaFakultatifKeluarCommandHandler : IRequestHandler<SaveNotaFakultatifKeluarCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveNotaFakultatifKeluarCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveNotaFakultatifKeluarCommandHandler(IDbContextPst contextPst,
            ILogger<SaveNotaFakultatifKeluarCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveNotaFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                
                var notaFakultatifKeluar = _contextPst.NotaFakultatifKeluar.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);
                
                if (notaFakultatifKeluar == null)
                {
                    throw new NotFoundException("Nota Fakultatif Keluar tidak ditemukan");
                }
                
                var details = _contextPst.DetailNotaFakultatifKeluar.Where(w =>
                    w.kd_cb == request.kd_cb && w.jns_tr == request.jns_tr &&
                    w.jns_nt_msk == request.jns_nt_msk && w.kd_thn == request.kd_thn &&
                    w.kd_bln == request.kd_bln && w.no_nt_msk == request.no_nt_msk &&
                    w.jns_nt_kel == request.jns_nt_kel && w.no_nt_kel == request.no_nt_kel).ToList();

                _contextPst.DetailNotaFakultatifKeluar.RemoveRange(details);

                var sequence = 1;
                foreach (var detail in request.Details)
                {
                    var detailNota = _mapper.Map<DetailNotaFakultatifKeluar>(detail);
                    detailNota.no_ang = (byte)sequence;
                    detailNota.kd_cb = request.kd_cb;
                    detailNota.jns_tr = request.jns_tr;
                    detailNota.jns_nt_msk = request.jns_nt_msk;
                    detailNota.kd_thn = request.kd_thn;
                    detailNota.kd_bln = request.kd_bln;
                    detailNota.no_nt_msk = request.no_nt_msk;
                    detailNota.jns_nt_kel = request.jns_nt_kel;
                    detailNota.no_nt_kel = request.no_nt_kel;
                    _contextPst.DetailNotaFakultatifKeluar.Add(detailNota);

                    sequence++;
                }

                notaFakultatifKeluar.ket_nt = request.ket_nt;
                notaFakultatifKeluar.ket_tc = request.ket_tc;

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}