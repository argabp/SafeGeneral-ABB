using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Services;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using ABB.Web.Modules.RegisterKlaim.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.RegisterKlaim
{
    public class RegisterKlaimViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public RegisterKlaimViewComponent(IMediator mediator, IMapper mapper, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync(RegisterKlaimModel model)
        {
            var registerKlaimViewModel = new RegisterKlaimViewModel();

            if (model == null) return View("_RegisterKlaim", registerKlaimViewModel);
            
            var command = _mapper.Map<GetRegisterKlaimQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            if (result == null)
            {
                registerKlaimViewModel.kd_cb = Request.Cookies["UserCabang"].Trim();
                registerKlaimViewModel.kd_cob = string.Empty;
                registerKlaimViewModel.kd_scob = string.Empty;
                registerKlaimViewModel.kd_thn = string.Empty;
                registerKlaimViewModel.no_kl = string.Empty;
                
                registerKlaimViewModel.flag_pol_lama = "N";
                registerKlaimViewModel.flag_tty_msk = "N";
                registerKlaimViewModel.no_updt = 0;
                registerKlaimViewModel.flag_settled = "N";
                registerKlaimViewModel.st_reg = "P";
                registerKlaimViewModel.kd_grp_bkl = "B";
                registerKlaimViewModel.flag_konv = "N";
                registerKlaimViewModel.tgl_input = DateTime.Now;
                registerKlaimViewModel.tgl_lns_prm = DateTime.Now;
                registerKlaimViewModel.tgl_reg = DateTime.Now;
                registerKlaimViewModel.tgl_updt = DateTime.Now;
                registerKlaimViewModel.kd_usr_input = _currentUserService.UserId;
                registerKlaimViewModel.kd_thn = DateTime.Now.ToString("yy");
                
                return View("_RegisterKlaim", registerKlaimViewModel);
            }
                
            _mapper.Map(result, registerKlaimViewModel);
            registerKlaimViewModel.kd_cb = registerKlaimViewModel.kd_cb.Trim();
            registerKlaimViewModel.kd_cob = registerKlaimViewModel.kd_cob.Trim();
            registerKlaimViewModel.kd_scob = registerKlaimViewModel.kd_scob.Trim();
            registerKlaimViewModel.kd_wilayah = registerKlaimViewModel.kd_wilayah.Trim();
            registerKlaimViewModel.IsEdit = true;

            return View("_RegisterKlaim", registerKlaimViewModel);
        }
    }
}