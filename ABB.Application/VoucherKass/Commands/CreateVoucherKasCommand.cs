using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;

namespace ABB.Application.VoucherKass.Commands
{
    public class CreateVoucherKasCommand : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string KodeKas { get; set; }
        public string DibayarKepada { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public decimal? TotalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public bool FlagPosting { get; set; }
        public string JenisPembayaran { get; set; }
        public string KodeUserInput { get; set; }

        public bool FlagSementara { get; set; }
        public string NoVoucherSementara { get; set; }
    }

    public class CreateVoucherKasCommandHandler : IRequestHandler<CreateVoucherKasCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateVoucherKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateVoucherKasCommand request, CancellationToken cancellationToken)
        {
            DateTime? tglVoucherFix = request.TanggalVoucher;

            if (request.TanggalVoucher.HasValue)
            {
                // PERBAIKAN: Tambahkan .ToLocalTime() sebelum .Date
                // Ini akan mengubah jam 17:00 (tgl 9) menjadi jam 00:00 (tgl 10) kembali.
                tglVoucherFix = request.TanggalVoucher.Value.ToLocalTime().Date;

                // OPSI CADANGAN (JURUS PAMUNGKAS):
                // Jika servernya settingan UTC (Cloud/Azure) dan ToLocalTime gak mempan,
                // Paksa tambah 7 jam (WIB):
                // tglVoucherFix = request.TanggalVoucher.Value.AddHours(7).Date;
            }

            // 2. Buat objek Entity
            var entity = new VoucherKasEntity
            {
                KodeCabang = request.KodeCabang,
                JenisVoucher = request.JenisVoucher,
                DebetKredit = request.DebetKredit,
                NoVoucher = request.NoVoucher,
                KodeAkun = request.KodeAkun,
                DibayarKepada = request.DibayarKepada,
                KodeMataUang = request.KodeMataUang,
                KodeKas = request.KodeKas,
                KeteranganVoucher = request.KeteranganVoucher,
                FlagPosting = request.FlagPosting,
                KodeUserInput = request.KodeUserInput,
                // Gunakan tanggal yang sudah diperbaiki
                TanggalVoucher = tglVoucherFix, 
                
                TotalVoucher = request.TotalVoucher,
                TotalDalamRupiah = request.TotalDalamRupiah,
                
                // Tanggal Input diset di sini (Server Time)
                TanggalInput = DateTime.Now, 
                FlagSementara = request.FlagSementara,
                NoVoucherSementara = request.NoVoucherSementara,
                
                JenisPembayaran = request.JenisPembayaran
            };

            // 3. Tambahkan entity baru ke DbContext
            _context.VoucherKas.Add(entity);

            // 4. Simpan perubahan ke database
            await _context.SaveChangesAsync(cancellationToken);

            // 5. Kembalikan Primary Key
            return entity.NoVoucher;
        }
    }
}