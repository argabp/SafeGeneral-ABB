using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesCSM.Commands
{
    public class ProsesGMMCommand : IRequest
    {
        public string Type { get; set; }
        
        public List<long> Id { get; set; }

        public string TipeTransaksi { get; set; }

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
                List<ViewSourceData> viewSourceDatas;

                if (request.Type == "All")
                    viewSourceDatas = _context.ViewSourceData.Where(w =>
                        request.KodeMetode == w.KodeMetode && request.TipeTransaksi == w.TipeTransaksi).ToList();
                else
                    viewSourceDatas = _context.ViewSourceData.Where(w => request.Id.Contains(w.Id)).ToList();
                
                var totalData = viewSourceDatas.Count;
            
                _progressBarDto.Total = totalData;
                _progressBarDto.Remaining = 0;
            
                for (int sequence = 0; sequence < totalData; sequence++)
                {
                    _progressBarDto.Remaining = sequence + 1;

                    await _db.QueryProc("spp_CalculationGMM3",
                        new
                        {
                            idNotaRisiko = viewSourceDatas[sequence].Id, PeriodeProses = viewSourceDatas[sequence].PeriodeProses.Date
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