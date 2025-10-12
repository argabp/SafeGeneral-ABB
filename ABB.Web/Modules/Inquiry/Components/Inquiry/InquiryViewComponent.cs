using System;
using System.Threading.Tasks;
using ABB.Application.Inquiries.Queries;
using ABB.Web.Modules.Inquiry.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.Inquiry
{
    public class InquiryViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InquiryViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(InquiryParameterViewModel model)
        {
            var InquiryViewModel = new InquiryViewModel();

            if (model == null) return View("_Inquiry", InquiryViewModel);
            
            var command = _mapper.Map<GetInquiryQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            if (result == null)
            {
                InquiryViewModel.no_pol = "00000";
                InquiryViewModel.no_updt = 0;
                InquiryViewModel.no_renew = 0;
                InquiryViewModel.thn_uw = 0;
                InquiryViewModel.no_endt = "0";
                InquiryViewModel.kd_grp_ttg = "9";
                InquiryViewModel.kd_grp_bank = "2";
                InquiryViewModel.st_pas = "O";
                InquiryViewModel.kd_grp_pas = "5";
                InquiryViewModel.kd_grp_bank = "6";
                InquiryViewModel.pst_share_bgu = 100;
                InquiryViewModel.faktor_prd = 100;
                InquiryViewModel.flag_konv = "N";
                InquiryViewModel.flag_reas = "N";
                InquiryViewModel.kd_grp_mkt = "M";
                InquiryViewModel.st_cover = "X";
                InquiryViewModel.wpc = 0;
                InquiryViewModel.st_aks = "1";
                InquiryViewModel.flag_dis_fleet = "N";
                InquiryViewModel.kd_thn = DateTime.Now.ToString("yy");
                InquiryViewModel.kd_cb = model.kd_cb.Trim();
                InquiryViewModel.kd_cob = string.Empty;
                InquiryViewModel.kd_scob = string.Empty;
                InquiryViewModel.tgl_mul_ptg = DateTime.Now;
                InquiryViewModel.tgl_akh_ptg = DateTime.Now.AddYears(1);
                InquiryViewModel.jk_wkt_ptg = Convert.ToInt16((InquiryViewModel.tgl_akh_ptg - InquiryViewModel.tgl_mul_ptg).TotalDays);
                InquiryViewModel.tgl_ttd = DateTime.Now;
                InquiryViewModel.tgl_closing = DateTime.Now;
                InquiryViewModel.tgl_input = DateTime.Now;
                
                return View("_Inquiry", InquiryViewModel);
            }
                
            _mapper.Map(result, InquiryViewModel);
            InquiryViewModel.kd_cb = InquiryViewModel.kd_cb.Trim();
            InquiryViewModel.kd_cob = InquiryViewModel.kd_cob.Trim();
            InquiryViewModel.kd_scob = InquiryViewModel.kd_scob.Trim();
            InquiryViewModel.IsEdit = true;

            return View("_Inquiry", InquiryViewModel);
        }
    }
}