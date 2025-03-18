using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Commands
{
    public class EditDetailMataUangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public decimal nilai_kurs { get; set; }
    }

    public class EditDetailMataUangCommandHandler : IRequestHandler<EditDetailMataUangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDetailMataUangCommandHandler> _logger;

        public EditDetailMataUangCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDetailMataUangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDetailMataUangCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var mataUang = dbContext.DetailMataUang.FirstOrDefault(w => w.kd_mtu == request.kd_mtu
                                                                            && w.tgl_akh == request.tgl_akh
                                                                            && w.tgl_mul == request.tgl_mul);

                if (mataUang != null)
                {
                    mataUang.nilai_kurs = request.nilai_kurs;
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