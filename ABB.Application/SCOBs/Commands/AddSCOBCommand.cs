using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.SCOBs.Commands
{
    public class AddSCOBCommand : IRequest, IMapFrom<SCOB>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_scob_ing { get; set; }

        public string kd_map_scob { get; set; }

        public string kd_sub_grp { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddSCOBCommand, SCOB>();
        }
    }

    public class AddSCOBCommandHandler : IRequestHandler<AddSCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public AddSCOBCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddSCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var scob = _mapper.Map<SCOB>(request);

            dbContext.SCOB.Add(scob);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}