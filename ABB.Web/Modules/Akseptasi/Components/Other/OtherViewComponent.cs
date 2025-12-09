using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Other
{
    public class OtherViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OtherViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            return View("~/Modules/Shared/Empty.cshtml");
        }
    }
}