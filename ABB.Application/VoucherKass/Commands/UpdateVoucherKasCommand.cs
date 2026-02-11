using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Commands
{
    public class UpdateVoucherKasCommand : IRequest
    {
        // Pastikan ID ada disini (sudah benar punya kamu)
        public long Id { get; set; } 

        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string KodeKas { get; set; }
        public string NoVoucher { get; set; } // Ini data target (baru)
        public string KodeAkun { get; set; }
        public string DibayarKepada { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool FlagPosting { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeUserUpdate { get; set; }
        public string JenisPembayaran { get; set; }
        public bool FlagSementara { get; set; }
        public string NoVoucherSementara { get; set; }
    }

    public class UpdateVoucherKasCommandHandler : IRequestHandler<UpdateVoucherKasCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateVoucherKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVoucherKasCommand request, CancellationToken cancellationToken)
        {
            DateTime? tglVoucherFix = request.TanggalVoucher;
            if (request.TanggalVoucher.HasValue)
            {
                tglVoucherFix = request.TanggalVoucher.Value.ToLocalTime().Date;
            }

            // --- PERBAIKAN DISINI ---
            // 1. Cari data berdasarkan ID (Bukan NoVoucher)
            // Karena ID tidak pernah berubah, sedangkan NoVoucher bisa berubah
            var entity = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (entity != null)
            {
                // 2. UPDATE NO VOUCHER DISINI
                // Timpa NoVoucher lama dengan yang baru (dari request)
                entity.NoVoucher = request.NoVoucher; 

                // Update field lainnya
                entity.KodeCabang = request.KodeCabang;
                entity.JenisVoucher = request.JenisVoucher;
                entity.DebetKredit = request.DebetKredit;
                entity.KodeAkun = request.KodeAkun;
                entity.DibayarKepada = request.DibayarKepada;
                entity.TanggalVoucher = tglVoucherFix;
                entity.KodeMataUang = request.KodeMataUang;
                entity.TotalVoucher = request.TotalVoucher;
                entity.TotalDalamRupiah = request.TotalDalamRupiah;
                entity.KeteranganVoucher = request.KeteranganVoucher;
                entity.FlagPosting = request.FlagPosting;
                entity.KodeKas = request.KodeKas;
                entity.TanggalUpdate = DateTime.Now; 
                entity.KodeUserUpdate = request.KodeUserUpdate;
                entity.JenisPembayaran = request.JenisPembayaran;
                entity.FlagSementara = request.FlagSementara;
                entity.NoVoucherSementara = request.NoVoucherSementara;

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}