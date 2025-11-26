using System.Threading.Tasks;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Web.Modules.PengajuanAkseptasi.Models;
using ABB.Web.Modules.RegisterKlaim.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.DokumenRegisterKlaim
{
    public class DokumenRegisterKlaimViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DokumenRegisterKlaimViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(RegisterKlaimModel model)
        {
            var command = _mapper.Map<GetDokumenRegisterKlaimsQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            return View("_DokumenRegisterKlaim", result);
        }
    }
}