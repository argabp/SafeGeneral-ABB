using System;
using System.Threading.Tasks;
using ABB.Api.Dto;
using ABB.Application.ProsesRekonsiliasi.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Api.Controllers
{
    [Route("[controller]")]
    public class RekonsiliasiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RekonsiliasiController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        
        [HttpPost("RekonsiliasiNasLife")]
        public async Task<IActionResult> RekonsiliasiNasLife([FromBody] RekonsiliasiDto model)
        {
            try
            {
                var dataRekonsiliasiNasLife = _mapper.Map<RekonsiliasiNasLifeCommand>(model);
                var resultDataRekonsiliasi = await _mediator.Send(dataRekonsiliasiNasLife);
                
                return Ok(resultDataRekonsiliasi);
            }
            catch (Exception e)
            {
                return Ok(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
    }
}