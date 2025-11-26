using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    // Command utama yang dikirim dari client
    public class CreatePenyelesaianPiutangNotaCommand : IRequest<int>
    {
        public string NoBukti { get; set; }
        public string FlagPembayaran { get; set; } = "NOTA";

        // Ini yang dikirim dari frontend: list of nota
        public List<PenyelesaianNotaItem> Data { get; set; } = new List<PenyelesaianNotaItem>();
    }

    // DTO untuk tiap item nota
    public class PenyelesaianNotaItem
    {
        public string NoNota { get; set; }
        public decimal TotalBayarOrg { get; set; }
        public decimal TotalBayarRp { get; set; }
        public string DebetKredit { get; set; }
        public string KodeMataUang { get; set; }
        public string KodeAkun { get; set; }
    }

    // Handler
    public class CreatePenyelesaianPiutangNotaCommandHandler : IRequestHandler<CreatePenyelesaianPiutangNotaCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public CreatePenyelesaianPiutangNotaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePenyelesaianPiutangNotaCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || !request.Data.Any())
                throw new Exception("Tidak ada data nota yang dikirim.");

            // Ambil nomor terakhir untuk voucher terkait
            var lastNo = await _context.EntriPenyelesaianPiutangTemp
                .Where(pb => pb.NoBukti == request.NoBukti)
                .OrderByDescending(pb => pb.No)
                .Select(pb => (int?)pb.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            int insertedCount = 0;

            foreach (var nota in request.Data)
            {
                lastNo++;
                var entity = new EntriPenyelesaianPiutangTemp
                {
                    NoBukti = request.NoBukti,
                    No = lastNo,
                    NoNota = nota.NoNota,
                    DebetKredit = nota.DebetKredit,
                    KodeMataUang = nota.KodeMataUang,
                    TotalBayarOrg = nota.TotalBayarOrg,
                    TotalBayarRp = nota.TotalBayarRp,
                    KodeAkun = nota.KodeAkun,
                    FlagPembayaran = request.FlagPembayaran,
                };

                _context.EntriPenyelesaianPiutangTemp.Add(entity);
                insertedCount++;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return insertedCount;
        }
    }
}
