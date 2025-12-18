using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class CreateJurnalMemorial117DetailCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }
        public string KodeUserInput { get; set; }
    }

    public class CreateJurnalMemorial117DetailCommandHandler : IRequestHandler<CreateJurnalMemorial117DetailCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public CreateJurnalMemorial117DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateJurnalMemorial117DetailCommand request, CancellationToken cancellationToken)
        {
            // Cari No urut terakhir
            var lastNo = await _context.JurnalMemorial117Detail
                .Where(x => x.NoVoucher == request.NoVoucher)
                .OrderByDescending(x => x.No)
                .Select(x => (int?)x.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var entity = new JurnalMemorial117Detail
            {
                NoVoucher = request.NoVoucher,
                No = lastNo + 1,
                KodeAkun = request.KodeAkun,
                NoNota = request.NoNota,
                KodeMataUang = request.KodeMataUang,
                NilaiDebet = request.NilaiDebet,
                NilaiDebetRp = request.NilaiDebetRp,
                NilaiKredit = request.NilaiKredit,
                NilaiKreditRp = request.NilaiKreditRp,
                KodeUserInput = request.KodeUserInput,
                TanggalUserInput = DateTime.Now
            };

            _context.JurnalMemorial117Detail.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.No;
        }
    }
}