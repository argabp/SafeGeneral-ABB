using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scriban;
using System.IO;

namespace ABB.Application.VoucherBanks.Queries
{
    public class GetVoucherBankByTanggalRangeQuery : IRequest<string> // UBAH: string hasil render HTML
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string KodeBank { get; set; } // filter opsional
        public string KeteranganBank { get; set; }
        public string DatabaseName { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetVoucherBankByTanggalRangeQueryHandler 
        : IRequestHandler<GetVoucherBankByTanggalRangeQuery, string> // UBAH: string
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherBankByTanggalRangeQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(GetVoucherBankByTanggalRangeQuery request, CancellationToken cancellationToken)
        {

            
            // Validasi tanggal
            if (request.TanggalAwal > request.TanggalAkhir)
            {
                (request.TanggalAwal, request.TanggalAkhir) = (request.TanggalAkhir, request.TanggalAwal);
            }

            // Query dasar
            var query = from v in _context.VoucherBank
                        join k in _context.KasBank
                            on v.KodeBank equals k.Kode into vk
                        from kas in vk.DefaultIfEmpty()
                        where v.TanggalVoucher.HasValue
                              && v.TanggalVoucher.Value >= request.TanggalAwal
                              && v.TanggalVoucher.Value <= request.TanggalAkhir
                        select new
                        {
                            v.NoVoucher,
                            v.TanggalVoucher,
                            v.KeteranganVoucher,
                            v.TotalVoucher,
                            v.DiterimaDari,
                            v.DebetKredit,
                            v.KodeBank,
                            v.JenisVoucher,
                            v.KodeCabang,
                            v.KodeMataUang,
                            v.TotalDalamRupiah,
                            v.JenisPembayaran,
                            Saldo = kas != null ? kas.Saldo : 0,
                            KeteranganKasBank = kas != null ? kas.Keterangan : ""
                        };

            // Filter opsional berdasarkan KodeBank
            if (!string.IsNullOrEmpty(request.KodeBank))
            {
                query = query.Where(v => v.KodeBank == request.KodeBank);
            }

            // Eksekusi query
            var result = await query
                .OrderBy(v => v.TanggalVoucher)
                .ToListAsync(cancellationToken);

            if (!result.Any())
                throw new Exception("Data voucher bank tidak ditemukan.");

            // =================================================================
            //  RENDER TEMPLATE SCRIBAN
            // =================================================================

            // 1️⃣ Siapkan helper format
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // 2️⃣ Buat detail tabel
            StringBuilder sb = new StringBuilder();
            int no = 1;
            foreach (var v in result)
            {
                sb.Append($@"
                    <tr>
                        <td class='center'>{no}</td>
                        <td>{fmtDate(v.TanggalVoucher)}</td>
                        <td>{v.NoVoucher}</td>
                        <td>{(string.IsNullOrWhiteSpace(v.DiterimaDari) ? "-" : v.DiterimaDari)}</td>
                        <td>{(string.IsNullOrWhiteSpace(v.KeteranganVoucher) ? "-" : v.KeteranganVoucher)}</td>
                        <td class='right'>{fmtNum(v.TotalVoucher)}</td>
                        <td>{v.KodeBank}</td>
                        <td>{v.JenisVoucher}</td>
                        <td>{v.KodeCabang}</td>
                    </tr>");
                no++;
            }

            // 3️⃣ Baca file template HTML
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "VoucherBank.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template laporan tidak ditemukan di {templatePath}");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath);

            // 4️⃣ Render Scriban
            var template = Template.Parse(htmlTemplate);

            string rendered = template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kode_bank = request.KodeBank ?? "-",
                keterangan_bank = request.KeteranganBank ?? "-",
                total_data = result.Count
            });

            return rendered;
        }
    }
}
