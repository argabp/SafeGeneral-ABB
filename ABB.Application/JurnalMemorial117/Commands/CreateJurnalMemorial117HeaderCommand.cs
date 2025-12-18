using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pakai entity JurnalMemorial117
using MediatR;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class CreateJurnalMemorial117HeaderCommand : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public string KodeUserInput { get; set; }
      
    }

    public class CreateJurnalMemorial117HeaderCommandHandler : IRequestHandler<CreateJurnalMemorial117HeaderCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateJurnalMemorial117HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateJurnalMemorial117HeaderCommand request, CancellationToken cancellationToken)
        {
            // Fix Tanggal (Timezone)
            DateTime? tglFix = request.Tanggal;
            if (request.Tanggal.HasValue)
            {
                tglFix = request.Tanggal.Value.ToLocalTime().Date;
            }

            var entity = new ABB.Domain.Entities.JurnalMemorial117
            {
                KodeCabang = request.KodeCabang,
                NoVoucher = request.NoVoucher,
                Tanggal = tglFix,
                Keterangan = request.Keterangan,
                KodeUserInput = request.KodeUserInput,
                TanggalInput = DateTime.Now,
                FlagPosting = false
            };

            _context.JurnalMemorial117.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.NoVoucher;
        }
    }
}