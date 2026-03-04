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
using ABB.Domain.Entities; // Sesuaikan namespace-nya

namespace ABB.Application.VoucherKass.Queries
{
    public class LaporanVoucherKasResponse
    {
        public string HtmlString { get; set; }
        public List<VoucherKasJoinDto> RawData { get; set; }
        public decimal SaldoAwal { get; set; }
        public decimal TotalDebet { get; set; }
        public decimal TotalKredit { get; set; }
        public decimal SaldoAkhir { get; set; }
        public string NmKas { get; set; }
        public string KodeKas { get; set; }
    }

    public class VoucherKasJoinDto
    {
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal? TotalVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string KodeKas { get; set; }
        public string NamaKas { get; set; }
    }

    public class GetVoucherKasByTanggalRangeQuery : IRequest<LaporanVoucherKasResponse>
    {
        public string DatabaseName { get; set; }
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string KodeKas { get; set; }
        public string KeteranganKas { get; set; }
        public string UserLogin { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetVoucherKasByTanggalRangeQueryHandler : IRequestHandler<GetVoucherKasByTanggalRangeQuery, LaporanVoucherKasResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherKasByTanggalRangeQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<LaporanVoucherKasResponse> Handle(GetVoucherKasByTanggalRangeQuery request, CancellationToken cancellationToken)
        {
            // =========================
            // EKSEKUSI STORED PROCEDURE
            // =========================
            var rawDbData = await _context.SpListingVoucherKasResults
                .FromSqlRaw("EXEC sp_ListingVoucherKas {0}, {1}, {2}, {3}",
                    request.TanggalAwal,
                    request.TanggalAkhir,
                    request.KodeKas ?? "",
                    request.KodeCabang ?? "")
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!rawDbData.Any())
                throw new Exception("Data Kas atau Voucher tidak ditemukan.");

            // Ekstrak Header
            var headerInfo = rawDbData.First();
            decimal saldoAwal = headerInfo.SaldoAwal;
            string namaKas = headerInfo.NamaKas;

            // Ekstrak Transaksi (Abaikan baris RowType = 0)
            var transaksiList = rawDbData.Where(x => x.RowType == 1).ToList();

            decimal totalDebet = transaksiList.Sum(x => x.Debet);
            decimal totalKredit = transaksiList.Sum(x => x.Kredit);
            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // FORMAT HELPERS
            // =========================
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // =========================
            // BUILD HTML TABLE
            // =========================
            StringBuilder sb = new StringBuilder();
            int no = 1;
            var dtoList = new List<VoucherKasJoinDto>();

            // Baris Saldo Awal
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6' style='background-color: #f9f9f9;'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>");

            // Baris Transaksi
            foreach (var v in transaksiList)
            {
                sb.Append($@"
                    <tr>
                        <td class='center' style='width: 40px;'>{no++}</td>
                        <td style='width: 90px;'>{fmtDate(v.TanggalVoucher)}</td>
                        <td style='width: 150px;'>{v.NoVoucher}</td>
                        <td class='right' style='width: 120px;'>{(v.Debet > 0 ? fmtNum(v.Debet) : "")}</td>
                        <td class='right' style='width: 120px;'>{(v.Kredit > 0 ? fmtNum(v.Kredit) : "")}</td>
                        <td>{v.KeteranganVoucher}</td>
                    </tr>");

                dtoList.Add(new VoucherKasJoinDto {
                    NoVoucher = v.NoVoucher,
                    TanggalVoucher = v.TanggalVoucher,
                    KeteranganVoucher = v.KeteranganVoucher,
                    TotalVoucher = v.Debet > 0 ? v.Debet : v.Kredit,
                    DebetKredit = v.Debet > 0 ? "D" : "K",
                    KodeKas = request.KodeKas,
                    NamaKas = namaKas
                });
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
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "VoucherKas.html");
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Template laporan tidak ditemukan.");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath, cancellationToken);
            var template = Template.Parse(htmlTemplate);

            string rendered = template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kas = request.KeteranganKas ?? "-",
                nama_kas = namaKas,
                kode_kas = request.KodeKas,
                user = request.UserLogin
            });

            return new LaporanVoucherKasResponse
            {
                HtmlString = rendered,
                RawData = dtoList,
                SaldoAwal = saldoAwal,
                TotalDebet = totalDebet,
                TotalKredit = totalKredit,
                SaldoAkhir = saldoAkhir,
                KodeKas = request.KodeKas,
                NmKas = namaKas
            };
        }
    }
}