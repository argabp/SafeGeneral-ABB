using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LKTs.Commands
{
    public class SaveLKTCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string st_tipe_dla { get; set; }

        public Int16 no_dla { get; set; }

        public string? ket_dla { get; set; }
    }

    public class SaveLKTCommandHandler : IRequestHandler<SaveLKTCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveLKTCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveLKTCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveLKTCommandHandler> logger, IMapper mapper)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveLKTCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var lks = dbContext.LKT.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts, request.st_tipe_dla, request.no_dla);

                lks.ket_dla = request.ket_dla;

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