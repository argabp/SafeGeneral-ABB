using System.Threading.Tasks;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Web.Modules.PengajuanAkseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PengajuanAkseptasi.Components.LampiranPengajuanAkseptasi
{
    public class LampiranPengajuanAkseptasiViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LampiranPengajuanAkseptasiViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(PengajuanAkseptasiModel model)
        {
            var command = _mapper.Map<GetPengajuanAkseptasiAttachmentQuery>(model);
            var result = await _mediator.Send(command);

            return View("_LampiranPengajuanAkseptasi", result);
        }
    }
}