using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;



namespace ABB.Application.VoucherBanks.Commands
{
    // Ini adalah "Surat Perintah" untuk membuat data baru
    public class CreateVoucherBankCommand : IRequest<string>
    {
        // Semua properti ini akan diisi dari ViewModel
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
    public class CreateVoucherBankCommandHandler : IRequestHandler<CreateVoucherBankCommand, string>
    {
        private readonly IDbContextPstNota _context;

        public CreateVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateVoucherBankCommand request, CancellationToken cancellationToken)
        {
            var entity = new VoucherBankEntity
            {
                // Memetakan semua data dari 'request' ke 'entity' baru
                KodeCabang = request.KodeCabang,
                JenisVoucher = request.JenisVoucher,
                DebetKredit = request.DebetKredit,
                NoVoucher = request.NoVoucher,
                KodeAkun = request.KodeAkun,
                DiterimaDari = request.DiterimaDari,
                TanggalVoucher = request.TanggalVoucher,
                KodeMataUang = request.KodeMataUang,
                TotalVoucher = request.TotalVoucher,
                TotalDalamRupiah = request.TotalDalamRupiah,
                KeteranganVoucher = request.KeteranganVoucher,
                FlagPosting = request.FlagPosting,
                KodeBank = request.KodeBank,
                JenisPembayaran = request.JenisPembayaran,
                NoBank = request.NoBank,
                TanggalInput = DateTime.Now // Otomatis mengisi tanggal input saat ini
            };

            _context.VoucherBank.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.NoVoucher; // Mengembalikan Primary Key
        }
    }
}