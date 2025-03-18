using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Commands
{
    public class AddDetailNotaCommand : IRequest, IMapFrom<DetailNota>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public byte no_ang { get; set; }

        public DateTime tgl_ang { get; set; }

        public DateTime tgl_jth_tempo { get; set; }

        public decimal pst_ang { get; set; }

        public decimal nilai_ang { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddDetailNotaCommand, DetailNota>();
        }
    }

    public class AddDetailNotaCommandHandler : IRequestHandler<AddDetailNotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDetailNotaCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddDetailNotaCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDetailNotaCommandHandler> logger, IMapper mapper)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddDetailNotaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var no_angsurans = dbContext.DetailNota.Where(w =>
                    w.kd_cb == request.kd_cb && w.jns_tr == request.jns_tr &&
                    w.jns_nt_msk == request.jns_nt_msk && w.kd_thn == request.kd_thn &&
                    w.kd_bln == request.kd_bln && w.no_nt_msk == request.no_nt_msk &&
                    w.jns_nt_kel == request.jns_nt_kel && w.no_nt_kel == request.no_nt_kel).ToList();

                byte no_angsuran;

                if (no_angsurans.Count > 0)
                {
                    byte maxNoAng = no_angsurans.Max(m => m.no_ang);
                    no_angsuran = (maxNoAng < 255) ? (byte)(maxNoAng + 1) : (byte)255; 
                }
                else
                    no_angsuran = 1;
                
                var detailNota = _mapper.Map<DetailNota>(request);

                detailNota.no_ang = no_angsuran;
                
                dbContext.DetailNota.Add(detailNota);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}