using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherBanks.Commands
{
    // Ini adalah "Surat Perintah" untuk memperbarui data
    public class UpdateVoucherBankCommand : IRequest
    {
        // Properti ini sama dengan Create, tapi NoVoucher digunakan untuk mencari data
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DiterimaDari { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool FlagPosting { get; set; }
        public string KodeBank { get; set; }
        public string NoBank { get; set; }
        public string JenisPembayaran { get; set; }
    }

    // Ini adalah "Petugas Pelaksana" untuk perintah di atas
    public class UpdateVoucherBankCommandHandler : IRequestHandler<UpdateVoucherBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public UpdateVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVoucherBankCommand request, CancellationToken cancellationToken)
        {
            // 1. Cari data yang ada di database berdasarkan Primary Key (NoVoucher)
            var entity = await _context.VoucherBank
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            // 2. Jika data ditemukan, update propertinya
            if (entity != null)
            {
                entity.KodeCabang = request.KodeCabang;
                entity.JenisVoucher = request.JenisVoucher;
                entity.DebetKredit = request.DebetKredit;
                entity.KodeAkun = request.KodeAkun;
                entity.DiterimaDari = request.DiterimaDari;
                entity.TanggalVoucher = request.TanggalVoucher;
                entity.KodeMataUang = request.KodeMataUang;
                entity.TotalVoucher = request.TotalVoucher;
                entity.TotalDalamRupiah = request.TotalDalamRupiah;
                entity.KeteranganVoucher = request.KeteranganVoucher;
                entity.FlagPosting = request.FlagPosting;
                entity.KodeBank = request.KodeBank;
                entity.NoBank = request.NoBank;
                entity.TanggalUpdate = DateTime.Now; // Otomatis mengisi tanggal update saat ini
                entity.JenisPembayaran = request.JenisPembayaran;
                // 3. Simpan perubahan ke database
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value; // Mengindikasikan proses selesai
        }
    }
}