using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.COBs.Commands
{
    public class EditCOBCommand : IRequest, IMapFrom<COB>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string nm_cob_ing { get; set; }

        public string kd_class { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditCOBCommand, COB>();
        }
    }

    public class EditCOBCommandHandler : IRequestHandler<EditCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public EditCOBCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            
            var entity = await dbContext.COB.FindAsync(request.kd_cob);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request, entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}