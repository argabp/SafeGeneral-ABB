using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pakai entity JurnalMemorial104
using MediatR;

namespace ABB.Application.JurnalMemorials104.Commands
{
    public class CreateJurnalMemorial104HeaderCommand : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public string KodeUserInput { get; set; }
      
    }

    public class CreateJurnalMemorial104HeaderCommandHandler : IRequestHandler<CreateJurnalMemorial104HeaderCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateJurnalMemorial104HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateJurnalMemorial104HeaderCommand request, CancellationToken cancellationToken)
        {
            // Fix Tanggal (Timezone)
            DateTime? tglFix = request.Tanggal;
            if (request.Tanggal.HasValue)
            {
                tglFix = request.Tanggal.Value.ToLocalTime().Date;
            }

            var entity = new ABB.Domain.Entities.JurnalMemorial104
            {
                KodeCabang = request.KodeCabang,
                NoVoucher = request.NoVoucher,
                Tanggal = tglFix,
                Keterangan = request.Keterangan,
                KodeUserInput = request.KodeUserInput,
                TanggalInput = DateTime.Now,
                FlagGL = false
            };

            _context.JurnalMemorial104.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.NoVoucher;
        }
    }
}