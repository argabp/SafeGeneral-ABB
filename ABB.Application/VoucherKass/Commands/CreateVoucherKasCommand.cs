using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;


namespace ABB.Application.VoucherKass.Commands
{
    // Ini adalah "surat perintah" yang sudah Anda buat
    public class CreateVoucherKasCommand : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DibayarKepada { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public decimal? TotalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public bool FlagPosting { get; set; }
        public string JenisPembayaran { get; set; }
    }

    // ---> INI BAGIAN YANG HILANG: "Petugas Pelaksana" <---
    public class CreateVoucherKasCommandHandler : IRequestHandler<CreateVoucherKasCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateVoucherKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateVoucherKasCommand request, CancellationToken cancellationToken)
        {
            // 1. Buat objek Entity dari data di dalam Command
            var entity = new VoucherKasEntity
            {
                KodeCabang = request.KodeCabang,
                JenisVoucher = request.JenisVoucher,
                DebetKredit = request.DebetKredit,
                NoVoucher = request.NoVoucher,
                KodeAkun = request.KodeAkun,
                DibayarKepada = request.DibayarKepada,
                KodeMataUang = request.KodeMataUang,
                KeteranganVoucher = request.KeteranganVoucher,
                FlagPosting = request.FlagPosting,
                TanggalVoucher = request.TanggalVoucher,
                TotalVoucher = request.TotalVoucher,
                TotalDalamRupiah = request.TotalDalamRupiah,
                TanggalInput = DateTime.Now,
                JenisPembayaran = request.JenisPembayaran

            };

            // 2. Tambahkan entity baru ke DbContext
            _context.VoucherKas.Add(entity);

            // 3. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Kembalikan Primary Key dari data yang baru dibuat
            return entity.NoVoucher;
        }
    }
}