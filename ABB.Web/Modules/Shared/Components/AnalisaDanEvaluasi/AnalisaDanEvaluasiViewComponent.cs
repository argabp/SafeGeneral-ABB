using System.Threading.Tasks;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Web.Modules.RegisterKlaim.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.AnalisaDanEvaluasi
{
    public class AnalisaDanEvaluasiViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AnalisaDanEvaluasiViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(RegisterKlaimModel model)
        {
            var analisaDanEvaluasiViewModel = new AnalisaDanEvaluasiViewModel();

            if (model == null) return View("_AnalisaDanEvaluasi", analisaDanEvaluasiViewModel);

            var command = _mapper.Map<GetAnalisaDanEvaluasiQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return View("_AnalisaDanEvaluasi", analisaDanEvaluasiViewModel);
            }

            _mapper.Map(result, analisaDanEvaluasiViewModel);

            return View("_AnalisaDanEvaluasi", analisaDanEvaluasiViewModel);
        }
    }
}