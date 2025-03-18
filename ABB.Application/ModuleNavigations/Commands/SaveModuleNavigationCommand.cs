using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.ModuleNavigations.Commands
{
    public class SaveModuleNavigationCommand : IRequest
    {
        public int ModuleId { get; set; }

        public List<int> Navigations { get; set; }
    }

    public class SaveModuleNavigationCommandHandler : IRequestHandler<SaveModuleNavigationCommand>
    {
        private readonly IDbContext _context;

        public SaveModuleNavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveModuleNavigationCommand request, CancellationToken cancellationToken)
        {
            var moduleNavigations = _context.ModuleNavigation.Where(roleModule => roleModule.ModuleId == request.ModuleId);
            
            _context.ModuleNavigation.RemoveRange(moduleNavigations);

            foreach (var navigation in request.Navigations)
                _context.ModuleNavigation.Add(new ModuleNavigation()
                {
                    ModuleId = request.ModuleId,
                    NavigationId = navigation
                });

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}