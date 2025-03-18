using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using ABB.Domain.Entities;

namespace ABB.Application.Modules.Commends
{
    public class EditModuleCommand : IRequest, IMapFrom<Module>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sequence { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditModuleCommand, Module>();
        }
    }

    public class EditModuleCommandHandler : IRequestHandler<EditModuleCommand>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public EditModuleCommandHandler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditModuleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Module.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}