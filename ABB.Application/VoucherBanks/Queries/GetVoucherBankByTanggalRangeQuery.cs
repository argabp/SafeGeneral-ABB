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

namespace ABB.Application.VoucherBanks.Queries
{

    public class LaporanVoucherBankResponse
    {
        public string HtmlString { get; set; }
        public List<VoucherBankDto> RawData { get; set; }
        public decimal SaldoAwal { get; set; }
        public decimal TotalDebet { get; set; }
        public decimal TotalKredit { get; set; }
        public decimal SaldoAkhir { get; set; }
         public string NamaBank { get; set; }
    }
    public class GetVoucherBankByTanggalRangeQuery : IRequest<LaporanVoucherBankResponse>
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
        : IRequestHandler<GetVoucherBankByTanggalRangeQuery, LaporanVoucherBankResponse>
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

        public async Task<LaporanVoucherBankResponse> Handle(
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
            var kasBank  = await _context.KasBank
                .Where(k => k.Kode == request.KodeBank && k.KodeCabang == request.KodeCabang && k.TipeKasBank == "BANK")
                .Select( k => new
                    {
                        k.NoPerkiraan,
                        k.Keterangan // ganti sesuai nama kolom aslinya
                    })
                .FirstOrDefaultAsync(cancellationToken);

                var KodeAkun = kasBank.NoPerkiraan;
                var NamaBank = kasBank.Keterangan;


                // =========================
                // AMBIL SALDO AWAL DARI ABBREKAP
                // =========================
                int tahun;
                int bulan;
                int tahunAwalSistem = 2026;
                if (request.TanggalAwal.Year == tahunAwalSistem && request.TanggalAwal.Month == 1)
                {
                    tahun = tahunAwalSistem;
                    bulan = 0;
                }
                else
                {
                    var periodeSebelumnya = request.TanggalAwal.AddMonths(-1);
                    tahun = periodeSebelumnya.Year;
                    bulan = periodeSebelumnya.Month;
                }

                var rekap = await _context.RekapJurnal
                    .Where(r =>
                        r.thn == tahun &&
                        r.bln == bulan &&
                        r.gl_akun == KodeAkun)
                    .ToListAsync(cancellationToken);

               decimal totalDebetAwal = rekap
                    .Where(r => r.gl_dk == "D")
                    .Sum(r => (decimal)r.gl_nilai_idr);

                decimal totalKreditAwal = rekap
                    .Where(r => r.gl_dk == "K")
                    .Sum(r => (decimal)r.gl_nilai_idr);

                decimal saldoAwal = totalDebetAwal - totalKreditAwal;

        

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
                            v.TotalDalamRupiah,
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

                var rawList = new List<VoucherBankDto>();

                foreach (var v in result)
                {
                    rawList.Add(new VoucherBankDto
                    {
                        NoVoucher = v.NoVoucher,
                        TanggalVoucher = v.TanggalVoucher,
                        KeteranganVoucher = v.KeteranganVoucher,
                        TotalVoucher = v.TotalDalamRupiah,
                        DebetKredit = v.DebetKredit
                    });
                }

            if (!rawList.Any())
                throw new Exception("Data voucher bank tidak ditemukan.");

            // =========================
            // FORMAT HELPER
            // =========================
            Func<decimal?, string> fmtNum =
                n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // =========================
            // TOTAL DEBET & KREDIT
            // =========================
           decimal totalDebet = rawList
                .Where(x => x.DebetKredit == "D")
                .Sum(x => x.TotalVoucher ?? 0);

            decimal totalKredit = rawList
                .Where(x => x.DebetKredit == "K")
                .Sum(x => x.TotalVoucher ?? 0);
            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // BUILD HTML TABLE
            // =========================
            StringBuilder sb = new StringBuilder();

            // SALDO AWAL
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>
            ");

            int nomor = 1;
            foreach (var v in rawList)
            {
                sb.Append($@"
                    <tr>
                        <td class='center'>{nomor++}</td>
                        <td class='center'>
                            {(v.TanggalVoucher.HasValue 
                                ? v.TanggalVoucher.Value.ToString("dd-MM-yy") 
                                : "-")}
                        </td>
                        <td>{v.NoVoucher}</td>
                        <td class='right'>
                            {(v.DebetKredit == "D" ? fmtNum(v.TotalVoucher ?? 0) : "")}
                        </td>
                        <td class='right'>
                            {(v.DebetKredit == "K" ? fmtNum(v.TotalVoucher ?? 0) : "")}
                        </td>
                        <td>{(string.IsNullOrWhiteSpace(v.KeteranganVoucher) ? "-" : v.KeteranganVoucher)}</td>
                    </tr>
                ");
               
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
                nama_bank = NamaBank ?? "-",
                keterangan_bank = request.KeteranganBank ?? "-"
            });

           return new LaporanVoucherBankResponse
            {
                HtmlString = rendered,
                RawData = rawList,
                SaldoAwal = saldoAwal,
                TotalDebet = totalDebet,
                TotalKredit = totalKredit,
                SaldoAkhir = saldoAkhir,
                NamaBank = NamaBank
            };
        }
    }
}
