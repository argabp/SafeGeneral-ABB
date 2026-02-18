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
using System.Collections.Generic;

namespace ABB.Application.VoucherKass.Queries
{
    public class VoucherKasJoinDto
    {
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal? TotalVoucher { get; set; }
        public string DibayarKepada { get; set; }
        public string DebetKredit { get; set; }
        public string KodeKas { get; set; }
        public string NamaKas { get; set; }
    }

    public class GetVoucherKasByTanggalRangeQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string KodeKas { get; set; }
        public string KeteranganKas { get; set; }
        public string UserLogin { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetVoucherKasByTanggalRangeQueryHandler
        : IRequestHandler<GetVoucherKasByTanggalRangeQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherKasByTanggalRangeQueryHandler(
            IDbContextPstNota context,
            IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(
            GetVoucherKasByTanggalRangeQuery request,
            CancellationToken cancellationToken)
        {

            var kodeAkun = await _context.KasBank
                .Where(k => k.Kode == request.KodeKas 
                        && k.KodeCabang == request.KodeCabang)
                .Select(k => k.NoPerkiraan)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrEmpty(kodeAkun))
                throw new Exception("Kode Akun tidak ditemukan pada data Kas.");


            // =========================
            // AMBIL SALDO AWAL
            // =========================
            decimal saldoAwal = await _context.KasBank
                .Where(k => k.Kode == request.KodeKas && k.KodeCabang == request.KodeCabang && k.NoPerkiraan == kodeAkun)
                .Select(k => k.Saldo ?? 0)
                .FirstOrDefaultAsync(cancellationToken);

            // =========================
            // QUERY MUTASI VOUCHER KAS (Perbaikan: Tanpa Join agar tidak looping)
            // =========================
            var queryBase = _context.VoucherKas
                .Where(v => v.TanggalVoucher.HasValue
                      && v.TanggalVoucher.Value >= request.TanggalAwal
                      && v.TanggalVoucher.Value <= request.TanggalAkhir
                      && v.KodeCabang == request.KodeCabang);

            if (!string.IsNullOrEmpty(request.KodeKas))
            {
                queryBase = queryBase.Where(x => x.KodeKas == request.KodeKas);
            }

            var voucherData = await queryBase
                .OrderBy(x => x.TanggalVoucher)
                .ToListAsync(cancellationToken);

            if (!voucherData.Any())
                throw new Exception("Data Voucher Kas tidak ditemukan.");

            // Mapping ke DTO tanpa join manual
            var vouchers = voucherData.Select(v => new VoucherKasJoinDto
            {
                NoVoucher = v.NoVoucher,
                TanggalVoucher = v.TanggalVoucher,
                KeteranganVoucher = v.KeteranganVoucher,
                TotalVoucher = v.TotalVoucher,
                DibayarKepada = v.DibayarKepada,
                DebetKredit = v.DebetKredit,
                KodeKas = v.KodeKas,
                NamaKas = request.KeteranganKas // Ambil dari parameter request
            }).ToList();

            // =========================
            // FORMAT HELPERS
            // =========================
            Func<DateTime?, string> fmtDate =
                d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";

            Func<decimal?, string> fmtNum =
                n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // =========================
            // KALKULASI TOTAL & SALDO
            // =========================
            decimal totalDebet = vouchers
                .Where(x => x.DebetKredit == "D")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal totalKredit = vouchers
                .Where(x => x.DebetKredit == "K")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // BUILD HTML TABLE (Details)
            // =========================
            StringBuilder sb = new StringBuilder();
            int no = 1;

            // Baris Saldo Awal
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6' style='background-color: #f9f9f9;'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>");

            foreach (var v in vouchers)
            {
                sb.Append($@"
                    <tr>
                        <td class='center' style='width: 40px;'>{no}</td>
                        <td style='width: 90px;'>{fmtDate(v.TanggalVoucher)}</td>
                        <td style='width: 150px;'>{v.NoVoucher}</td>
                        <td class='right' style='width: 120px;'>{(v.DebetKredit == "D" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td class='right' style='width: 120px;'>{(v.DebetKredit == "K" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td>{v.KeteranganVoucher}</td>
                    </tr>");
                no++;
            }

            // Baris Total
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='3' class='right'>TOTAL MUTASI</td>
                    <td class='right'>{fmtNum(totalDebet)}</td>
                    <td class='right'>{fmtNum(totalKredit)}</td>
                    <td></td>
                </tr>");

            // Baris Saldo Akhir
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6' style='background-color: #f2f2f2;'>Saldo Akhir : {fmtNum(saldoAkhir)}</td>
                </tr>");

            // =========================
            // RENDER SCRIBAN
            // =========================
            string templatePath = Path.Combine(
                _environment.ContentRootPath,
                "Modules", "Reports", "Templates", "VoucherKas.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template laporan tidak ditemukan di {templatePath}");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath, cancellationToken);
            var template = Template.Parse(htmlTemplate);

            return template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kas = request.KeteranganKas ?? "-",
                user = request.UserLogin
            });
        }
    }
}