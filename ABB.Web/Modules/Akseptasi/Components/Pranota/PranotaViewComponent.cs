using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Pranota
{
    public class PranotaViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PranotaViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiPranotaParameterViewModel model)
        {
            return View("~/Modules/Shared/Empty.cshtml");
        }
    }
}