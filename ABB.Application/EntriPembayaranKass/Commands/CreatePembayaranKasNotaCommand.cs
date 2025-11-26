using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    // 🧩 DTO untuk tiap nota yang dikirim dari frontend (JS)
    public class PembayaranKasNotaItem
    {
        public string NoNota { get; set; }
        public decimal TotalBayarOrg { get; set; }
        public decimal TotalBayarRp { get; set; }
        public string DebetKredit { get; set; }
        public string KodeMataUang { get; set; }
         public int? Kurs { get; set; }
    }

    // 🧩 Command utama
    public class CreatePembayaranKasNotaCommand : IRequest<int>
    {
        // Nomor voucher (wajib)
        public string NoVoucher { get; set; }

        // Default flag
        public string FlagPembayaran { get; set; } = "NOTA";

        // Data nota yang dikirim dari frontend
        public List<PembayaranKasNotaItem> Data { get; set; } = new List<PembayaranKasNotaItem>();
    }

    // 🧩 Handler
    public class CreatePembayaranKasNotaCommandHandler : IRequestHandler<CreatePembayaranKasNotaCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public CreatePembayaranKasNotaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePembayaranKasNotaCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.NoVoucher))
                throw new Exception("Nomor voucher tidak boleh kosong.");

            if (request.Data == null || !request.Data.Any())
                throw new Exception("Tidak ada data nota yang dikirim.");

            // Ambil nomor terakhir berdasarkan voucher
            var lastNo = await _context.EntriPembayaranKasTemp
                .Where(pb => pb.NoVoucher == request.NoVoucher)
                .OrderByDescending(pb => pb.No)
                .Select(pb => (int?)pb.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            int insertedCount = 0;

            foreach (var item in request.Data)
            {
                lastNo++;

                var entity = new EntriPembayaranKasTemp
                {
                    NoVoucher = request.NoVoucher,
                    No = lastNo,
                    NoNota4 = item.NoNota,
                    DebetKredit = item.DebetKredit,
                    KodeMataUang = item.KodeMataUang,
                    TotalBayar = item.TotalBayarOrg,
                    TotalDlmRupiah = item.TotalBayarRp,
                    FlagPembayaran = request.FlagPembayaran,
                    Kurs = item.Kurs,
                    FlagPosting = "N"
                };

                _context.EntriPembayaranKasTemp.Add(entity);
                insertedCount++;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return insertedCount;
        }
    }
}
