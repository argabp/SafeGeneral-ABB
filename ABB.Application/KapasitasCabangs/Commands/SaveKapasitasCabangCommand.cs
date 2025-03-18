using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KapasitasCabangs.Commands
{
    public class SaveKapasitasCabangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public decimal nilai_kapasitas { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public decimal? nilai_kl { get; set; }
    }

    public class SaveKapasitasCabangCommandHandler : IRequestHandler<SaveKapasitasCabangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKapasitasCabangCommandHandler> _logger;

        public SaveKapasitasCabangCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKapasitasCabangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKapasitasCabangCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kapasitasCabang = dbContext.KapasitasCabang.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                             && w.kd_cob == request.kd_cob
                                                                             && w.kd_scob == request.kd_scob
                                                                             && w.thn == request.thn);

                if (kapasitasCabang == null)
                {
                    dbContext.KapasitasCabang.Add(new KapasitasCabang()
                    {
                        kd_cb = request.kd_cb,
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        thn = request.thn,
                        nilai_kapasitas = request.nilai_kapasitas,
                        tgl_input = request.tgl_input,
                        kd_usr_input = request.kd_usr_input,
                        nilai_kl = request.nilai_kl
                    });
                }
                else
                {
                    kapasitasCabang.nilai_kapasitas = request.nilai_kapasitas;
                    kapasitasCabang.tgl_input = request.tgl_input;
                    kapasitasCabang.kd_usr_input = request.kd_usr_input;
                    kapasitasCabang.nilai_kl = request.nilai_kl;
                }
                
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}