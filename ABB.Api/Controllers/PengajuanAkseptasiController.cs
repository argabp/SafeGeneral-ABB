using System;
using System.Threading.Tasks;
using ABB.Api.Dto;
using ABB.Application.Approval.Commands;
using ABB.Application.PengajuanAkseptasi.Commands;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ABB.Api.Controllers
{
    [Route("[controller]")]
    public class PengajuanAkseptasiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public PengajuanAkseptasiController(IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _configuration = configuration;
        }
        
        [HttpPost("DataAkseptasi")]
        public async Task<IActionResult> DataAkseptasi([FromBody] DataAkseptasiDto model)
        {
            try
            {
                var dataAkseptasiNasLife = _mapper.Map<DataAkseptasiNasLifeCommand>(model);
                var resultDataAkseptasi = await _mediator.Send(dataAkseptasiNasLife);

                if (resultDataAkseptasi.remark == "Need Confirmation NasLife")
                {
                    var confirmNasLife = _mapper.Map<ConfirmNasLifeCommand>(model);
                    confirmNasLife.remark = model.keterangan;
                    confirmNasLife.status = "1";
                    var result = await _mediator.Send(confirmNasLife);
                    resultDataAkseptasi.remark = result.msg;
                    resultDataAkseptasi.status = result.status;
                }
                
                return Ok(resultDataAkseptasi);
            }
            catch (Exception e)
            {
                return Ok(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }

        [HttpPost("ApprovalAkseptasi")]
        public async Task<IActionResult> ApprovalAkseptasi([FromBody] ApprovalAkseptasiDto model)
        {
            try
            {
                var authKey = _configuration.GetSection("AuthKey").Value;
                
                if (authKey != model.authkey)
                    return Ok(new { model.no_sppa, remark = "Auth Key is not correct" });

                var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

                var url = location.AbsoluteUri;

                var validateNasLife = await _mediator.Send(new ValidateApprovalNasLifeCommand(){ no_sppa = model.no_sppa, EndPoint = url});
                
                if(string.IsNullOrWhiteSpace(validateNasLife)){
                
                    var approvalNasLife = _mapper.Map<ApprovalAkseptasiNasLifeCommand>(model);
                    approvalNasLife.EndPoint = url;
                    approvalNasLife.flagApiFrom = "ApprovalAkseptasi";
                    var result = await _mediator.Send(approvalNasLife);

                    return Ok(new { model.no_sppa, remark = result });
                }
                
                return Ok(new { model.no_sppa, remark = validateNasLife });
                    
            }
            catch (Exception e)
            {
                return Ok(e.InnerException == null ? e.Message : e.InnerException.Message);
            }
        }
    }
}