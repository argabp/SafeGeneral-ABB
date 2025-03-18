using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.SCOBs.Commands
{
    public class EditSCOBCommand : IRequest, IMapFrom<SCOB>
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
            profile.CreateMap<EditSCOBCommand, SCOB>();
        }
    }

    public class EditSCOBCommandHandler : IRequestHandler<EditSCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public EditSCOBCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditSCOBCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.SCOB.FindAsync(request.kd_cob, request.kd_scob);
            if (entity == null)
                throw new NotFoundException();

            _mapper.Map(request, entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}