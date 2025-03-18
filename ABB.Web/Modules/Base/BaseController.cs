using ABB.Application.Common.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ABB.Web.Modules.Base
{
    public class BaseController : Controller
    {
        private ISender _mediator;
        private IMapper _mapper;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
        protected ICurrentUserService CurrentUser => HttpContext.RequestServices.GetService<ICurrentUserService>();
    }
}