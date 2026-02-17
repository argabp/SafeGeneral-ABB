using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelCSM.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelCSM.Commands
{
    public class CancelGMMCommand : IRequest
    {
        public string Type { get; set; }
        
        public List<CancelCSMDto> Datas { get; set; }

        public string KodeMetode { get; set; }
    }
    
    public class CancelGMMCommandHandler : IRequestHandler<CancelGMMCommand>
    {
        private readonly IDbConnectionCSM _db;
        private readonly IDbContextCSM _context;
        private readonly ProgressBarDto _progressBarDto;
        private readonly ILogger<CancelGMMCommandHandler> _logger;

        public CancelGMMCommandHandler(IDbConnectionCSM db, IDbContextCSM context, ProgressBarDto progressBarDto,
            ILogger<CancelGMMCommandHandler> logger)
        {
            _db = db;
            _context = context;
            _progressBarDto = progressBarDto;
            _logger = logger;
        }
    
        public async Task<Unit> Handle(CancelGMMCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<CancelCSMDto> viewSourceDatas;

                if (request.Type == "All")
                    viewSourceDatas = _context.ViewSourceDataCancel.Where(w =>
                        request.KodeMetode == w.KodeMetode).Select(s => new CancelCSMDto()
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

                    await _db.QueryProc("spp_CancelationGMM",
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