using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NomorRegistrasiPolis.Commands
{
    public class SaveNomorRegistrasiPolisCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string no_reg { get; set; }
        public string ket_no_reg { get; set; }
    }

    public class SaveNomorRegistrasiPolisCommandHandler : IRequestHandler<SaveNomorRegistrasiPolisCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveNomorRegistrasiPolisCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveNomorRegistrasiPolisCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveNomorRegistrasiPolisCommandHandler> logger, IMapper mapper)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveNomorRegistrasiPolisCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var nomorRegistrasiPolis = dbContext.NomorRegistrasiPolis.Find(
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt);

                if (nomorRegistrasiPolis == null)
                {
                    throw new NullReferenceException();
                }
                
                nomorRegistrasiPolis.no_reg = request.no_reg;
                nomorRegistrasiPolis.ket_no_reg = request.ket_no_reg;

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