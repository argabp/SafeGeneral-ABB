using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.PostingJurnalMemorial104.Commands
{
    public class PostingJurnalMemorial104Command : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
    }

    public class PostingJurnalMemorial104CommandHandler : IRequestHandler<PostingJurnalMemorial104Command>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public PostingJurnalMemorial104CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingJurnalMemorial104Command request, CancellationToken cancellationToken)
        {
            // 1. Ambil semua entity VoucherKas yang NoVoucher-nya ada di dalam daftar request.Data
            var vouchersToUpdate = await _context.JurnalMemorial104
                .Where(v => request.Data.Contains(v.NoVoucher))
                .ToListAsync(cancellationToken);

            // 2. Loop melalui hasil yang ditemukan dan update flag_posting-nya
            foreach (var voucher in vouchersToUpdate)
            {
                voucher.FlagGL = true; // Atau 'Y', sesuaikan dengan standar Anda
                voucher.TanggalPosting = DateTime.Now;
            }

            // 3. Simpan semua perubahan ke database dalam satu kali perintah
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}