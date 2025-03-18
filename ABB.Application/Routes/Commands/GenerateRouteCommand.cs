using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Routes.Commands
{
    public class GenerateRouteCommand : IRequest
    {
    }

    public class GenerateRouteCommandHandler : IRequestHandler<GenerateRouteCommand>
    {
        private readonly IDbContext _context;
        private readonly ILog _log;
        private readonly IAssemblyHelper _assembly;

        public GenerateRouteCommandHandler(IDbContext context, ILog log, IAssemblyHelper assembly)
        {
            _context = context;
            _log = log;
            _assembly = assembly;
        }

        public async Task<Unit> Handle(GenerateRouteCommand request, CancellationToken cancellationToken)
        {
            InsertRoute();
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private void InsertRoute()
        {
            try
            {
                var data = _assembly.GetAssemblyRoutes().Distinct();
                foreach (var item in data.Distinct())
                {
                    var route = new Route() { Action = item.Action, Controller = item.Controller };
                    var exist = _context.Route.Any(a => a.Action == route.Action
                                                        && a.Controller == route.Controller);
                    if (!exist)
                        _context.Route.Add(route);
                }
            }

            catch (Exception ex)
            {
                _log.Error(ex, MethodBase.GetCurrentMethod());
            }
        }
    }
}