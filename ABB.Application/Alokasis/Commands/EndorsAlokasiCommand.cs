using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Commands
{
    public class EndorsAlokasiCommand : IRequest<(string, string, string)>
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
        
        public string ket_endt { get; set; }
    }

    public class EndorsAlokasiCommandHandler : IRequestHandler<EndorsAlokasiCommand, (string, string, string)>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<EndorsAlokasiCommandHandler> _logger;

        public EndorsAlokasiCommandHandler(IDbContextPst dbContextPst, IDbConnectionPst dbConnectionPst,
            ILogger<EndorsAlokasiCommandHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(EndorsAlokasiCommand request, CancellationToken cancellationToken)
        {
            var result = await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spe_ri05e_13x",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt,
                        request.no_rsk, request.kd_endt, request.no_updt_reas,
                        request.ket_endt
                    })).FirstOrDefault(), _logger);

            if (result.Item1 == "1")
            {
                var entity = await _dbContextPst.Alokasi.FindAsync(request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt,
                    request.no_rsk, request.kd_endt, request.no_updt_reas);

                if (entity == null)
                {
                    throw new NullReferenceException("Alokasi is not found");
                }
            
                entity.ket_endt = request.ket_endt;

                await _dbContextPst.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}