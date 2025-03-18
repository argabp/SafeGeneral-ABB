using System;
using System.Threading.Tasks;
using ABB.Api.Dto;
using ABB.Application.ApprovalNotaDinas.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Api.Controllers
{
    [Route("[controller]")]
    public class NotaDinasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NotaDinasController> _logger;

        public NotaDinasController(IMediator mediator, ILogger<NotaDinasController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpPost("DataAlokasi")]
        public async Task<IActionResult> DataAlokasi([FromBody] DataAlokasiDto model)
        {
            try
            {
                var resultDataAlokasi = await _mediator.Send(new DataAlokasiNasLifeCommand() { id_nds = model.id_nds, remark = model.keterangan});

                return Ok(resultDataAlokasi);
            }
            catch (Exception e)
            {
                return Ok(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
        
        [HttpPost("UpdatePembayaran")]
        public async Task<IActionResult> UpdatePembayaran([FromForm] UpdatePembayaranDto model)
        {
            try
            {
                _logger.LogInformation("Object In: " + JsonConvert.SerializeObject(model));
                
                var resultUpdatePembayaran = await _mediator.Send(new UpdatePembayaranNasLifeCommand()
                {
                    id_nds = model.id_nds, no_bukti_bayar = model.keterangan, tgl_bayar = model.tgl_status,
                    Attachments = model.Attachments
                });

                return Ok(resultUpdatePembayaran);
            }
            catch (Exception e)
            {
                return Ok(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
    }
}