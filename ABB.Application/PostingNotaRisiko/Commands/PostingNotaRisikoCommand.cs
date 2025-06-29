using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaRisiko.Commands
{
    public class PostingNotaRisikoCommand : IRequest
    {
        public string TipeTransaksi { get; set; }

        public DateTime PeriodeProses { get; set; }
    }
    
    public class PostingNotaRisikoCommandHandler : IRequestHandler<PostingNotaRisikoCommand>
    {
        private readonly IDbConnectionCSM _db;
        private readonly ILogger<PostingNotaRisikoCommandHandler> _logger;

        public PostingNotaRisikoCommandHandler(IDbConnectionCSM db,
            ILogger<PostingNotaRisikoCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<Unit> Handle(PostingNotaRisikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _db.QueryProc("sp_PostingNotaRisiko",
                    new
                    {
                        request.TipeTransaksi, periodeProses = request.PeriodeProses.Date
                    });
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw;
            }

            return Unit.Value;
        }
    }
}