using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.Cabangs.Commands
{
    public class EditCabangCommand : IRequest, IMapFrom<Cabang>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string almt { get; set; }

        public string kt { get; set; }

        public string kd_pos { get; set; }

        public string no_tlp { get; set; }

        public string npwp { get; set; }

        public string no_fax { get; set; }

        public string email { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditCabangCommand, Cabang>();
        }
    }

    public class EditCabangCommandHandler : IRequestHandler<EditCabangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public EditCabangCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditCabangCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.Cabang.FindAsync(request.kd_cb);
            if (entity == null)
                throw new NotFoundException();

            _mapper.Map(request, entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}