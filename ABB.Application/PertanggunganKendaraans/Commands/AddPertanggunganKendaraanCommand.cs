using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class AddPertanggunganKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public string? desk { get; set; }

        public Int16? jml_hari { get; set; }

        public string? ket_klasula { get; set; }

        public string? flag_tjh { get; set; }

        public string? flag_rscc { get; set; }

        public string? flag_banjir { get; set; }

        public string? flag_accessories { get; set; }

        public string? flag_lain_lain01 { get; set; }
        
        public string? flag_lain_lain02 { get; set; }
    }

    public class AddPertanggunganKendaraanCommandHandler : IRequestHandler<AddPertanggunganKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddPertanggunganKendaraanCommandHandler> _logger;

        public AddPertanggunganKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddPertanggunganKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddPertanggunganKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var pertanggunganKendaraan = new PertanggunganKendaraan()
                {
                    kd_cob = request.kd_cob,
                    kd_scob = "00",
                    kd_jns_ptg = request.kd_jns_ptg,
                    desk = request.desk,
                    flag_accessories = request.flag_accessories,
                    flag_banjir = request.flag_banjir,
                    flag_lain_lain01 = request.flag_lain_lain01,
                    flag_lain_lain02 = request.flag_lain_lain02,
                    flag_rscc = request.flag_rscc,
                    flag_tjh = request.flag_tjh,
                    jml_hari = request.jml_hari,
                    ket_klasula = request.ket_klasula
                };

                dbContext.PertanggunganKendaraan.Add(pertanggunganKendaraan);

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