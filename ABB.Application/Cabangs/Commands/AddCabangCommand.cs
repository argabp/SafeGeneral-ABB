using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.Cabangs.Commands
{
    public class AddCabangCommand : IRequest, IMapFrom<Cabang>
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
            profile.CreateMap<AddCabangCommand, Cabang>();
        }
    }

    public class AddCabangCommandHandler : IRequestHandler<AddCabangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public AddCabangCommandHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddCabangCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            
            var cabang = _mapper.Map<Cabang>(request);

            dbContext.Cabang.Add(cabang);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}