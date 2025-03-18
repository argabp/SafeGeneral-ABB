using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using ABB.Domain.Entities;

namespace ABB.Application.Modules.Commends
{
    public class AddModuleCommand : IRequest, IMapFrom<Module>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sequence { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddModuleCommand, Module>();
        }
    }

    public class AddModuleCommandHandler : IRequestHandler<AddModuleCommand>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public AddModuleCommandHandler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddModuleCommand request, CancellationToken cancellationToken)
        {
            var module = _mapper.Map<Module>(request);

            _context.Module.Add(module);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}