using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Commands
{
    public class EditLimitKlaimDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }
        
        public decimal nilai_limit_akhir { get; set; }
    }

    public class EditLimitKlaimDetilCommandHandler : IRequestHandler<EditLimitKlaimDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLimitKlaimDetilCommandHandler> _logger;

        public EditLimitKlaimDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLimitKlaimDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLimitKlaimDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaimDetil = dbContext.LimitKlaimDetail.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.thn == request.thn
                    && w.kd_user == request.kd_user);

                if (limitKlaimDetil != null)
                {
                    limitKlaimDetil.nilai_limit_awal = request.nilai_limit_awal;
                    limitKlaimDetil.nilai_limit_akhir = request.nilai_limit_akhir;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

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