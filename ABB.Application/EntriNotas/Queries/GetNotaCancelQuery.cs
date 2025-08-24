using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Queries
{
    public class GetNotaCancelQuery : IRequest<NotaDto>, IMapFrom<Nota>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetNotaCancelQueryHandler : IRequestHandler<GetNotaCancelQuery, NotaDto>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetNotaCancelQueryHandler> _logger;

        public GetNotaCancelQueryHandler(IDbContextFactory dbContextFactory, IMapper mapper,
            ILogger<GetNotaCancelQueryHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NotaDto> Handle(GetNotaCancelQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var nota = dbContext.Nota.FirstOrDefault(w => w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob &&
                                                     w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn &&
                                                     w.no_pol == request.no_pol && w.no_updt == request.no_updt &&
                                                     w.flag_cancel.Trim() == "Y");

                if (nota == null)
                    throw new Exception("Nota Cancel is not found");
                
                return _mapper.Map<NotaDto>(nota);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}