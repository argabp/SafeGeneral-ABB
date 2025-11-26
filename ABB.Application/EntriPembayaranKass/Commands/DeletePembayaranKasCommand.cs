using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    public class DeletePembayaranKasCommand : IRequest
    {
        public int  No { get; set; }          // Id detail pembayaran
        public string NoVoucher { get; set; }   // Nomor voucher header
    }

    public class DeletePembayaranKasCommandHandler : IRequestHandler<DeletePembayaranKasCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeletePembayaranKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePembayaranKasCommand request, CancellationToken cancellationToken)
        {
            if (request.No <= 0 || string.IsNullOrEmpty(request.NoVoucher))
            {
                 return Unit.Value;
            }

            // Cari detail pembayaran berdasarkan Id dan NoVoucher
            var entity = await _context.EntriPembayaranKasTemp
                .FirstOrDefaultAsync(v => v.No == request.No && v.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity != null)
            {
                _context.EntriPembayaranKasTemp.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
