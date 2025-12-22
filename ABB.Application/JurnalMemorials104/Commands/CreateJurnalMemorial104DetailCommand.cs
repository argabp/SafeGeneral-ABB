using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.JurnalMemorials104.Commands
{
    public class CreateJurnalMemorial104DetailCommand : IRequest<int>
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
        public string KeteranganDetail { get; set; }
    }

    public class CreateJurnalMemorial104DetailCommandHandler : IRequestHandler<CreateJurnalMemorial104DetailCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public CreateJurnalMemorial104DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateJurnalMemorial104DetailCommand request, CancellationToken cancellationToken)
        {
            // Cari No urut terakhir
            var lastNo = await _context.DetailJurnalMemorial104
                .Where(x => x.NoVoucher == request.NoVoucher)
                .OrderByDescending(x => x.No)
                .Select(x => (int?)x.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var entity = new DetailJurnalMemorial104
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
                TanggalUserInput = DateTime.Now,
                KeteranganDetail = request.KeteranganDetail
            };

            _context.DetailJurnalMemorial104.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.No;
        }
    }
}