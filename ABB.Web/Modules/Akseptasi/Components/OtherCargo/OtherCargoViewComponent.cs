using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.OtherCargo
{
    public class OtherCargoViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OtherCargoViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            var akseptasiCargoViewModel = new AkseptasiResikoOtherCargoViewModel()
            {
                kd_cb = model.kd_cb.Trim(),
                kd_cob = model.kd_cob.Trim(),
                kd_scob = model.kd_scob.Trim(),
                kd_thn = model.kd_thn.Trim(),
                no_updt = model.no_updt,
                no_aks = model.no_aks
            };
            
            var cargoCommand = _mapper.Map<GetAkseptasiOtherCargoQuery>(model);
            cargoCommand.DatabaseName = Request.Cookies["DatabaseValue"];
            var cargoResult = await _mediator.Send(cargoCommand);

            if (cargoResult == null)
            {
                akseptasiCargoViewModel.grp_kond = "003";
                akseptasiCargoViewModel.IsNewOther = true;

                return View("_OtherCargo", akseptasiCargoViewModel);
            }
        
            _mapper.Map(cargoResult, akseptasiCargoViewModel);
            akseptasiCargoViewModel.kd_cb = akseptasiCargoViewModel.kd_cb.Trim();
            akseptasiCargoViewModel.kd_cob = akseptasiCargoViewModel.kd_cob.Trim();
            akseptasiCargoViewModel.kd_scob = akseptasiCargoViewModel.kd_scob.Trim();
            
            return View("_OtherCargo", akseptasiCargoViewModel);
        }
    }
}