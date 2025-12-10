using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Commands
{
    // Ini adalah "Surat Perintah" untuk memperbarui data
    public class UpdateVoucherKasCommand : IRequest
    {
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DibayarKepada { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool FlagPosting { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string JenisPembayaran { get; set; }
    }

    // Ini adalah "Petugas Pelaksana" untuk perintah di atas
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
            // 1. Cari data yang ada di database berdasarkan Primary Key (NoVoucher)
            var entity = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            // 2. Jika data ditemukan, update propertinya
            if (entity != null)
            {
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
                entity.TanggalUpdate = DateTime.Now; 
                entity.JenisPembayaran = request.JenisPembayaran;
                // 3. Simpan perubahan ke database
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value; // Mengindikasikan proses selesai
        }
    }
}