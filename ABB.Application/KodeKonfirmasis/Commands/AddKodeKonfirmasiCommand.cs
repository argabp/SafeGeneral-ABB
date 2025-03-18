using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ABB.Application.KodeKonfirmasis.Commands
{
    public class AddKodeKonfirmasiCommand : IRequest<KodeKonfirmasi>, IMapFrom<KodeKonfirmasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string kd_konfirm { get; set; }

        public string flag_polis { get; set; }

        public string UserId { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddKodeKonfirmasiCommand, KodeKonfirmasi>();
        }
    }

    public class AddKodeKonfirmasiCommandHandler : IRequestHandler<AddKodeKonfirmasiCommand, KodeKonfirmasi>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;

        public AddKodeKonfirmasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
        }

        public async Task<KodeKonfirmasi> Handle(AddKodeKonfirmasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                request.kd_thn = DateTime.Now.ToString("yy");
                
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var kd_konfirm = (await _connectionFactory.QueryProc<string>("spe_rf42e_00", new
                    {
                        kode = request.kd_scob + request.no_aks + request.flag_polis
                    }))
                    .First();

                var kodeKonfirmasi = _mapper.Map<KodeKonfirmasi>(request);

                var user = await dbContext.User.FindAsync(request.UserId);
                
                kodeKonfirmasi.tgl_input = DateTime.Now;
                kodeKonfirmasi.kd_konfirm = kd_konfirm.Split(",")[1];
                kodeKonfirmasi.kd_usr_input = user.FirstName;

                dbContext.KodeKonfirmasi.Add(kodeKonfirmasi);

                await dbContext.SaveChangesAsync(cancellationToken);

                return kodeKonfirmasi;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}