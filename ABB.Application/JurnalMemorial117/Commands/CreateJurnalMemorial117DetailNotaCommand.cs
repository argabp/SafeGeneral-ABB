using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class CreateJurnalMemorial117DetailNotaCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
        public List<JurnalNotaItem> Data { get; set; } = new List<JurnalNotaItem>();
        public string KodeUserInput { get; set; }
    }

    public class JurnalNotaItem
    {
        public string NoNota { get; set; }
        public string KodeAkun { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiKredit { get; set; }
        // Tambahkan properti lain (NilaiRp) jika perlu
    }

    public class CreateJurnalMemorial117DetailNotaCommandHandler : IRequestHandler<CreateJurnalMemorial117DetailNotaCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public CreateJurnalMemorial117DetailNotaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateJurnalMemorial117DetailNotaCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || !request.Data.Any())
                throw new Exception("Tidak ada data nota.");

            var lastNo = await _context.JurnalMemorial117Detail
                .Where(x => x.NoVoucher == request.NoVoucher)
                .OrderByDescending(x => x.No)
                .Select(x => (int?)x.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            int count = 0;
            foreach (var item in request.Data)
            {
                lastNo++;
                var entity = new JurnalMemorial117Detail
                {
                    NoVoucher = request.NoVoucher,
                    No = lastNo,
                    NoNota = item.NoNota,
                    KodeAkun = item.KodeAkun,
                    KodeMataUang = item.KodeMataUang,
                    NilaiDebet = item.NilaiDebet,
                    NilaiKredit = item.NilaiKredit,
                    KodeUserInput = request.KodeUserInput,
                    TanggalUserInput = DateTime.Now
                };
                _context.JurnalMemorial117Detail.Add(entity);
                count++;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return count;
        }
    }
}