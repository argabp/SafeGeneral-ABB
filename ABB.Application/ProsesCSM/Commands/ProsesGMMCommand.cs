using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesCSM.Queries;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesCSM.Commands
{
    public class ProsesGMMCommand : IRequest
    {
        public string Type { get; set; }
        
        public List<ProsesCSMDto> Datas { get; set; }

        public string KodeMetode { get; set; }
    }
    
    public class ProsesGMMCommandHandler : IRequestHandler<ProsesGMMCommand>
    {
        private readonly IDbConnectionCSM _db;
        private readonly IDbContextCSM _context;
        private readonly ProgressBarDto _progressBarDto;
        private readonly ILogger<ProsesGMMCommandHandler> _logger;

        public ProsesGMMCommandHandler(IDbConnectionCSM db, IDbContextCSM context, ProgressBarDto progressBarDto,
            ILogger<ProsesGMMCommandHandler> logger)
        {
            _db = db;
            _context = context;
            _progressBarDto = progressBarDto;
            _logger = logger;
        }
    
        public async Task<Unit> Handle(ProsesGMMCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                List<ProsesCSMDto> viewSourceDatas;

                if (request.Type == "All")
                    viewSourceDatas = _context.ViewSourceData.Where(w =>
                        request.KodeMetode == w.KodeMetode).Select(s => new ProsesCSMDto()
                        {
                            Id = s.Id,
                            PeriodeProses = s.PeriodeProses
                        }).ToList();
                else
                {
                    viewSourceDatas = request.Datas;
                }
                
                var totalData = viewSourceDatas.Count;
            
                _progressBarDto.Total = totalData;
                _progressBarDto.Remaining = 0;
            
                var sequence = 0;

                foreach (var data in viewSourceDatas)
                {
                    sequence++;
                    _progressBarDto.Remaining = sequence;

                    await _db.QueryProc("spp_CalculationGMM3",
                        new
                        {
                            idNotaRisiko = data.Id, PeriodeProses = data.PeriodeProses.Date
                        });   
                }
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