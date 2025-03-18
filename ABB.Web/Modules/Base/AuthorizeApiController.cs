using ABB.Application.Common.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ABB.Web.Modules.Base
{
    public class AuthorizeApiController : Controller
    {
        [Authorize]
        [Route("api/[controller]/[Action]")]
        [ApiController]
        public class AuthorizedApiController : ControllerBase
        {
            private ISender _mediator;
            private IMapper _mapper;

            protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
            protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
            protected ICurrentUserService CurrentUser => HttpContext.RequestServices.GetService<ICurrentUserService>();
        }
    }
}