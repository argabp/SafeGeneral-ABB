using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaPerSubCOBs.Commands
{
    public class SaveBiayaPerSubCOBCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_prm { get; set; }

        public decimal nilai_bia_pol { get; set; }

        public decimal nilai_bia_adm { get; set; }

        public decimal nilai_min_form { get; set; }

        public decimal nilai_maks_plafond { get; set; }
    }

    public class SaveBiayaPerSubCOBCommandHandler : IRequestHandler<SaveBiayaPerSubCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveBiayaPerSubCOBCommandHandler> _logger;

        public SaveBiayaPerSubCOBCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveBiayaPerSubCOBCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveBiayaPerSubCOBCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var biayaPerSubCOB = dbContext.BiayaPerSubCOB.FirstOrDefault(w => w.kd_mtu == request.kd_mtu
                                                                             && w.kd_cob == request.kd_cob
                                                                             && w.kd_scob == request.kd_scob);

                if (biayaPerSubCOB == null)
                {
                    dbContext.BiayaPerSubCOB.Add(new BiayaPerSubCOB()
                    {
                        kd_mtu = request.kd_mtu,
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        nilai_min_prm = request.nilai_min_prm,
                        nilai_bia_pol = request.nilai_bia_pol,
                        nilai_bia_adm = request.nilai_bia_adm,
                        nilai_min_form = request.nilai_min_form,
                        nilai_maks_plafond = request.nilai_maks_plafond,
                    });
                }
                else
                {
                    biayaPerSubCOB.nilai_min_prm = request.nilai_min_prm;
                    biayaPerSubCOB.nilai_bia_pol = request.nilai_bia_pol;
                    biayaPerSubCOB.nilai_bia_adm = request.nilai_bia_adm;
                    biayaPerSubCOB.nilai_min_form = request.nilai_min_form;
                    biayaPerSubCOB.nilai_maks_plafond = request.nilai_maks_plafond;
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