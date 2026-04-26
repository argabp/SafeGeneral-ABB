using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Commands
{
    public class SaveAlokasiCommand : IRequest, IMapFrom<Alokasi>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }

        public string kd_mtu_prm { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public string flag_closing { get; set; }

        public DateTime tgl_closing { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAlokasiCommand, Alokasi>();
        }
    }

    public class SaveAlokasiCommandHandler : IRequestHandler<SaveAlokasiCommand>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveAlokasiCommandHandler> _logger;

        public SaveAlokasiCommandHandler(IDbContextPst dbContextPst, IMapper mapper,
            ILogger<SaveAlokasiCommandHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAlokasiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var entity = await _dbContextPst.Alokasi.FindAsync(request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt,
                    request.no_rsk, request.kd_endt, request.no_updt_reas);

                if (entity == null)
                {
                    var Alokasi = _mapper.Map<Alokasi>(request);
                    _dbContextPst.Alokasi.Add(Alokasi);

                    await _dbContextPst.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.kd_mtu_prm = request.kd_mtu_prm;
                    entity.nilai_ttl_ptg = request.nilai_ttl_ptg;
                    entity.nilai_prm = request.nilai_prm;

                    if (entity.kd_cb.Length != 5)
                        for (int sequence = entity.kd_cb.Length; sequence < 5; sequence++)
                        {
                            entity.kd_cb += " ";
                        }

                    if (entity.kd_cob.Length != 2)
                        for (int sequence = entity.kd_cob.Length; sequence < 2; sequence++)
                        {
                            entity.kd_cob += " ";
                        }

                    if (entity.kd_scob.Length != 5)
                        for (int sequence = entity.kd_scob.Length; sequence < 5; sequence++)
                        {
                            entity.kd_scob += " ";
                        }

                    await _dbContextPst.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }, _logger);
        }
    }
}