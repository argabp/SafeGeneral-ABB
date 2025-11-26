using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Commands
{
    public class DeletePembayaranBankCommand : IRequest
    {
        public int  No { get; set; }          // Id detail pembayaran
        public string NoVoucher { get; set; }   // Nomor voucher header
    }

    public class DeletePembayaranBankCommandHandler : IRequestHandler<DeletePembayaranBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeletePembayaranBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePembayaranBankCommand request, CancellationToken cancellationToken)
        {
            if (request.No <= 0 || string.IsNullOrEmpty(request.NoVoucher))
            {
                 return Unit.Value;
            }

            // Cari detail pembayaran berdasarkan Id dan NoVoucher
            var entity = await _context.EntriPembayaranBankTemp
                .FirstOrDefaultAsync(v => v.No == request.No && v.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity != null)
            {
                _context.EntriPembayaranBankTemp.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
