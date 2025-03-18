using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.COBs.Commands
{
    public class AddCOBCommand : IRequest, IMapFrom<COB>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string nm_cob_ing { get; set; }

        public string kd_class { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddCOBCommand, COB>();
        }
    }

    public class AddCOBCommandHandler : IRequestHandler<AddCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public AddCOBCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var cob = _mapper.Map<COB>(request);

            dbContext.COB.Add(cob);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}