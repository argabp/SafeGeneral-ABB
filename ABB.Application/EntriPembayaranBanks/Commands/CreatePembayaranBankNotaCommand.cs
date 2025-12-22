using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ABB.Application.EntriPembayaranBanks.Commands
{
    // Class helper untuk menampung data dari setiap baris
   public class PembayaranBankNotaItem
    {
        public string NoNota { get; set; }
        public int TotalBayarOrg { get; set; }
        public decimal TotalBayarRp { get; set; }
        public string DebetKredit { get; set; }
        public string KodeMataUang { get; set; }
        public int? Kurs { get; set; }
        public string KodeAkun { get; set; }
    }

    public class CreatePembayaranBankNotaCommand : IRequest<int>
    {
        // Properti Data sekarang adalah List dari objek kompleks
       public string NoVoucher { get; set; }

        // Default flag
        public string FlagPembayaran { get; set; } = "NOTA";
        
         public string KodeUserInput { get; set; }
        public DateTime? TanggalInput { get; set; }

        // Data nota yang dikirim dari frontend
        public List<PembayaranBankNotaItem> Data { get; set; } = new List<PembayaranBankNotaItem>();
    }

    public class CreatePembayaranBankNotaCommandHandler : IRequestHandler<CreatePembayaranBankNotaCommand, int>
    {
        private readonly IDbContextPstNota _context;
        public CreatePembayaranBankNotaCommandHandler(IDbContextPstNota context) { _context = context; }

        public async Task<int> Handle(CreatePembayaranBankNotaCommand request, CancellationToken cancellationToken)
        {
             if (string.IsNullOrEmpty(request.NoVoucher))
                throw new Exception("Nomor voucher tidak boleh kosong.");

            if (request.Data == null || !request.Data.Any())
                throw new Exception("Tidak ada data nota yang dikirim.");

            var lastNo = await _context.EntriPembayaranBankTemp
                .Where(pb => pb.NoVoucher == request.NoVoucher)
                 .OrderByDescending(pb => pb.No)
                .Select(pb => (int?)pb.No)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            int insertedCount = 0;

            foreach (var nota in request.Data) // Loop melalui List<NotaToSave>
            {
                lastNo++;
                var entity = new EntriPembayaranBankTemp
                {
                   NoVoucher = request.NoVoucher,
                    No = lastNo,
                    NoNota4 = nota.NoNota,
                    DebetKredit = nota.DebetKredit,
                    KodeMataUang = nota.KodeMataUang,
                    TotalBayar = nota.TotalBayarOrg,
                    TotalDlmRupiah = nota.TotalBayarRp,
                    KodeAkun = nota.KodeAkun,
                    FlagPembayaran = request.FlagPembayaran,
                    TanggalInput = DateTime.Now, 
                    KodeUserInput = request.KodeUserInput,
                    Kurs = nota.Kurs,
                    
                };
                _context.EntriPembayaranBankTemp.Add(entity);
                insertedCount++;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return insertedCount;
        }
    }
}