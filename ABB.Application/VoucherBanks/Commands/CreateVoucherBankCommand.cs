using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;

namespace ABB.Application.VoucherBanks.Commands
{
    // [PERBAIKAN 1] Ubah IRequest<string> menjadi IRequest<long>
    public class CreateVoucherBankCommand : IRequest<long>
    {
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
        public string KodeUserInput { get; set; }
        public string JenisPembayaran { get; set; }

        public bool FlagSementara { get; set; }
        public string NoVoucherSementara { get; set; }
    }

    // [PERBAIKAN 2] Ubah IRequestHandler<..., string> menjadi IRequestHandler<..., long>
    public class CreateVoucherBankCommandHandler : IRequestHandler<CreateVoucherBankCommand, long>
    {
        private readonly IDbContextPstNota _context;

        public CreateVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        // [PERBAIKAN 3] Ubah Task<string> menjadi Task<long>
        public async Task<long> Handle(CreateVoucherBankCommand request, CancellationToken cancellationToken)
        {
            DateTime? tglVoucherFix = request.TanggalVoucher;

            if (request.TanggalVoucher.HasValue)
            { 
                tglVoucherFix = request.TanggalVoucher.Value.ToLocalTime().Date;
            }

            var entity = new VoucherBankEntity
            {
                KodeCabang = request.KodeCabang,
                JenisVoucher = request.JenisVoucher,
                DebetKredit = request.DebetKredit,
                NoVoucher = request.NoVoucher,
                KodeAkun = request.KodeAkun,
                DiterimaDari = request.DiterimaDari,
                TanggalVoucher = tglVoucherFix,
                KodeMataUang = request.KodeMataUang,
                TotalVoucher = request.TotalVoucher,
                TotalDalamRupiah = request.TotalDalamRupiah,
                KeteranganVoucher = request.KeteranganVoucher,
                FlagPosting = request.FlagPosting,
                KodeBank = request.KodeBank?.Trim(),
                KodeUserInput = request.KodeUserInput,
                JenisPembayaran = request.JenisPembayaran,
                NoBank = request.NoBank,
                TanggalInput = DateTime.Now ,
                FlagSementara = request.FlagSementara,
                NoVoucherSementara = request.NoVoucherSementara,
            };

            _context.VoucherBank.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // [PERBAIKAN 4] Gunakan 'Id' (Huruf Besar) sesuai Entity
            return entity.Id; 
        }
    }
}