using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.CancelPostingVoucherBank.Commands
{
    public class CancelPostingVoucherBankCommand : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
    }

    public class CancelPostingVoucherBankCommandHandler : IRequestHandler<CancelPostingVoucherBankCommand>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public CancelPostingVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingVoucherBankCommand request, CancellationToken cancellationToken)
        {
            // 1. Ambil semua entity VoucherKas yang NoVoucher-nya ada di dalam daftar request.Data
            var vouchersToUpdate = await _context.VoucherBank
                .Where(v => request.Data.Contains(v.NoVoucher))
                .ToListAsync(cancellationToken);

            // 2. Loop melalui hasil yang ditemukan dan update flag_posting-nya
            foreach (var voucher in vouchersToUpdate)
            {
                voucher.FlagPosting = false; // Atau 'Y', sesuaikan dengan standar Anda
            }

            // 3. Simpan semua perubahan ke database dalam satu kali perintah
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}