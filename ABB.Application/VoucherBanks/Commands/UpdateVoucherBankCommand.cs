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
        public long Id { get; set; }
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
        public string KodeUserUpdate { get; set; }
        public string JenisPembayaran { get; set; }
         public bool FlagSementara { get; set; }
        public string NoVoucherSementara { get; set; }
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

            DateTime? tglVoucherFix = request.TanggalVoucher;

            if (request.TanggalVoucher.HasValue)
            {
                tglVoucherFix = request.TanggalVoucher.Value.ToLocalTime().Date;
            }

            // 1. Cari data yang ada di database berdasarkan Primary Key (NoVoucher)
            var entity = await _context.VoucherBank
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            // 2. Jika data ditemukan, update propertinya
            if (entity != null)
            {
                entity.NoVoucher = request.NoVoucher; 
                entity.KodeCabang = request.KodeCabang?.Trim();
                entity.JenisVoucher = request.JenisVoucher?.Trim();
                entity.DebetKredit = request.DebetKredit?.Trim();
                entity.KodeAkun = request.KodeAkun;
                entity.DiterimaDari = request.DiterimaDari;
                entity.TanggalVoucher = tglVoucherFix;
                entity.KodeMataUang = request.KodeMataUang;
                entity.TotalVoucher = request.TotalVoucher;
                entity.TotalDalamRupiah = request.TotalDalamRupiah;
                entity.KeteranganVoucher = request.KeteranganVoucher;
                entity.FlagPosting = request.FlagPosting;
                entity.KodeBank = request.KodeBank?.Trim();
                entity.NoBank = request.NoBank;
                entity.KodeUserUpdate = request.KodeUserUpdate;
                entity.TanggalUpdate = DateTime.Now; // Otomatis mengisi tanggal update saat ini
                entity.JenisPembayaran = request.JenisPembayaran;
                entity.FlagSementara = request.FlagSementara;
                entity.NoVoucherSementara = request.NoVoucherSementara?.Trim();
                // 3. Simpan perubahan ke database
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value; // Mengindikasikan proses selesai
        }
    }
}