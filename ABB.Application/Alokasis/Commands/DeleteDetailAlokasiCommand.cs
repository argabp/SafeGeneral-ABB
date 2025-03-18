using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Commands
{
    public class DeleteDetailAlokasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_updt_reas { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_grp_sor { get; set; }

        public string kd_rk_sor { get; set; }
        public string kd_grp_sb_bis { get; set; }
    }

    public class DeleteDetailAlokasiCommandHandler : IRequestHandler<DeleteDetailAlokasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;

        public DeleteDetailAlokasiCommandHandler(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteDetailAlokasiCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
            var entity = await dbContext.DetailAlokasi.FindAsync(request.kd_cb, 
                request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                request.no_rsk, request.kd_endt, request.no_updt_reas, request.kd_jns_sor, 
                request.kd_grp_sor, request.kd_rk_sor, request.kd_grp_sb_bis);

            if (entity == null)
                throw new NotFoundException();

            dbContext.DetailAlokasi.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}