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
            // =========================
            // SALDO AWAL DARI KASBANK
            // =========================
            decimal saldoAwal = await _context.KasBank
                .Where(k => k.Kode == request.KodeKas)
                .Select(k => k.Saldo ?? 0)
                .FirstOrDefaultAsync(cancellationToken);

            // =========================
            // MUTASI VOUCHER KAS
            // =========================
            var query =
                from v in _context.VoucherKas
                join k in _context.KasBank
                    on v.KodeKas equals k.Kode
                where v.TanggalVoucher.HasValue
                      && v.TanggalVoucher.Value >= request.TanggalAwal
                      && v.TanggalVoucher.Value <= request.TanggalAkhir
                select new VoucherKasJoinDto
                {
                    NoVoucher = v.NoVoucher,
                    TanggalVoucher = v.TanggalVoucher,
                    KeteranganVoucher = v.KeteranganVoucher,
                    TotalVoucher = v.TotalVoucher,
                    DibayarKepada = v.DibayarKepada,
                    DebetKredit = v.DebetKredit,
                    KodeKas = v.KodeKas,
                    NamaKas = k.Keterangan
                };

            if (!string.IsNullOrEmpty(request.KodeKas))
                query = query.Where(x => x.KodeKas == request.KodeKas);

            var vouchers = await query
                .OrderBy(x => x.TanggalVoucher)
                .ToListAsync(cancellationToken);

            if (!vouchers.Any())
                throw new Exception("Data Voucher Kas tidak ditemukan.");

            // =========================
            // FORMAT
            // =========================
            Func<DateTime?, string> fmtDate =
                d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";

            Func<decimal?, string> fmtNum =
                n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // =========================
            // TOTAL
            // =========================
            decimal totalDebet = vouchers
                .Where(x => x.DebetKredit == "D")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal totalKredit = vouchers
                .Where(x => x.DebetKredit == "K")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // BUILD TABLE
            // =========================
            StringBuilder sb = new StringBuilder();
            int no = 1;

            // SALDO AWAL
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>
            ");

            foreach (var v in vouchers)
            {
                sb.Append($@"
                    <tr>
                        <td class='center'>{no}</td>
                        <td>{fmtDate(v.TanggalVoucher)}</td>
                        <td>{v.NoVoucher}</td>
                        <td class='right'>{(v.DebetKredit == "D" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td class='right'>{(v.DebetKredit == "K" ? fmtNum(v.TotalVoucher) : "")}</td>
                        <td>{v.KeteranganVoucher}</td>
                    </tr>
                ");
                no++;
            }

            // TOTAL
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='3' class='right'>TOTAL</td>
                    <td class='right'>{fmtNum(totalDebet)}</td>
                    <td class='right'>{fmtNum(totalKredit)}</td>
                    <td></td>
                </tr>
            ");

            // SALDO AKHIR
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6'>Saldo Akhir : {fmtNum(saldoAkhir)}</td>
                </tr>
            ");

            // =========================
            // RENDER SCRIBAN
            // =========================
            string templatePath = Path.Combine(
                _environment.ContentRootPath,
                "Modules", "Reports", "Templates", "VoucherKas.html");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath, cancellationToken);
            var template = Template.Parse(htmlTemplate);

            return template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kas = request.KeteranganKas,
                user = request.UserLogin
            });
        }
    }
}
