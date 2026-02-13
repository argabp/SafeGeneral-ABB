using System;
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
    public class GetVoucherBankByTanggalRangeQuery : IRequest<string>
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string KodeBank { get; set; }
        public string KeteranganBank { get; set; }
        public string DatabaseName { get; set; }
        public string UserLogin { get; set; }

        public string KodeCabang { get; set; }
    }

    public class GetVoucherBankByTanggalRangeQueryHandler
        : IRequestHandler<GetVoucherBankByTanggalRangeQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherBankByTanggalRangeQueryHandler(
            IDbContextPstNota context,
            IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(
            GetVoucherBankByTanggalRangeQuery request,
            CancellationToken cancellationToken)
        {
            // =========================
            // VALIDASI TANGGAL
            // =========================
            if (request.TanggalAwal > request.TanggalAkhir)
            {
                (request.TanggalAwal, request.TanggalAkhir)
                    = (request.TanggalAkhir, request.TanggalAwal);
            }

            // =========================
            // AMBIL SALDO AWAL DARI KASBANK
            // =========================
            decimal saldoAwal = await _context.KasBank
                .Where(k => k.Kode == request.KodeBank && k.KodeCabang == request.KodeCabang)
                .Select(k => k.Saldo ?? 0)
                .FirstOrDefaultAsync(cancellationToken);

            // =========================
            // QUERY MUTASI VOUCHER BANK
            // =========================
            var query = from v in _context.VoucherBank
                        where v.TanggalVoucher.HasValue
                              && v.TanggalVoucher.Value >= request.TanggalAwal
                              && v.TanggalVoucher.Value <= request.TanggalAkhir
                              && v.KodeCabang == request.KodeCabang
                        select new
                        {
                            v.NoVoucher,
                            v.TanggalVoucher,
                            v.KeteranganVoucher,
                            v.TotalVoucher,
                            v.DebetKredit,
                            v.KodeBank
                        };

            if (!string.IsNullOrEmpty(request.KodeBank))
            {
                query = query.Where(x => x.KodeBank == request.KodeBank);
            }

            var result = await query
                .OrderBy(x => x.TanggalVoucher)
                .ToListAsync(cancellationToken);

            if (!result.Any())
                throw new Exception("Data voucher bank tidak ditemukan.");

            // =========================
            // FORMAT HELPER
            // =========================
            Func<decimal?, string> fmtNum =
                n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // =========================
            // TOTAL DEBET & KREDIT
            // =========================
            decimal totalDebet = result
                .Where(x => x.DebetKredit == "D")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal totalKredit = result
                .Where(x => x.DebetKredit == "K")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // BUILD HTML TABLE
            // =========================
            StringBuilder sb = new StringBuilder();
            int no = 1;

            // SALDO AWAL
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='5'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>
            ");

            foreach (var v in result)
            {
                sb.Append($@"
                    <tr>
                        <td class='center'>{no}</td>
                        <td>{v.NoVoucher}</td>
                        <td class='right'>{(v.DebetKredit == "D" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td class='right'>{(v.DebetKredit == "K" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td>{(string.IsNullOrWhiteSpace(v.KeteranganVoucher) ? "-" : v.KeteranganVoucher)}</td>
                    </tr>
                ");
                no++;
            }

            // TOTAL
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='2' class='right'>TOTAL</td>
                    <td class='right'>{fmtNum(totalDebet)}</td>
                    <td class='right'>{fmtNum(totalKredit)}</td>
                    <td></td>
                </tr>
            ");

            // SALDO AKHIR
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='5'>Saldo Akhir : {fmtNum(saldoAkhir)}</td>
                </tr>
            ");

            // =========================
            // RENDER SCRIBAN
            // =========================
            string templatePath = Path.Combine(
                _environment.ContentRootPath,
                "Modules", "Reports", "Templates", "VoucherBank.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException(
                    $"Template laporan tidak ditemukan di {templatePath}");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(htmlTemplate);

            string rendered = template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kode_bank = request.KodeBank ?? "-",
                keterangan_bank = request.KeteranganBank ?? "-"
            });

            return rendered;
        }
    }
}
