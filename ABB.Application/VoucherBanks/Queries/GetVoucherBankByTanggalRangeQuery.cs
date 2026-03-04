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
using ABB.Domain.Entities; // Sesuaikan

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

    public class GetVoucherBankByTanggalRangeQueryHandler : IRequestHandler<GetVoucherBankByTanggalRangeQuery, LaporanVoucherBankResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherBankByTanggalRangeQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<LaporanVoucherBankResponse> Handle(GetVoucherBankByTanggalRangeQuery request, CancellationToken cancellationToken)
        {
            // =========================
            // EKSEKUSI STORED PROCEDURE
            // =========================
            var rawDbData = await _context.SpListingVoucherBankResults
                .FromSqlRaw("EXEC sp_ListingVoucherBank {0}, {1}, {2}, {3}",
                    request.TanggalAwal,
                    request.TanggalAkhir,
                    request.KodeBank ?? "",
                    request.KodeCabang ?? "")
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!rawDbData.Any())
                throw new Exception("Data Bank atau Voucher tidak ditemukan.");

            // Ambil Info Header dari Baris Pertama (RowType 0 pasti ada)
            var headerInfo = rawDbData.First();
            decimal saldoAwal = headerInfo.SaldoAwal;
            string namaBank = headerInfo.NamaBank;

            // Filter Khusus Data Transaksi
            var transaksiList = rawDbData.Where(x => x.RowType == 1).ToList();

            decimal totalDebet = transaksiList.Sum(x => x.Debet);
            decimal totalKredit = transaksiList.Sum(x => x.Kredit);
            decimal saldoAkhir = saldoAwal + totalDebet - totalKredit;

            // =========================
            // FORMAT HELPER & BUILD HTML
            // =========================
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";
            StringBuilder sb = new StringBuilder();

            // Row Saldo Awal
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6'>Saldo Awal : {fmtNum(saldoAwal)}</td>
                </tr>
            ");

            int nomor = 1;
            var rawListDto = new List<VoucherBankDto>();

            // Row Transaksi
            foreach (var v in transaksiList)
            {
                sb.Append($@"
                    <tr>
                        <td class='center'>{nomor++}</td>
                        <td class='center'>{(v.TanggalVoucher.HasValue ? v.TanggalVoucher.Value.ToString("dd-MM-yy") : "-")}</td>
                        <td>{v.NoVoucher}</td>
                        <td class='right'>{(v.Debet > 0 ? fmtNum(v.Debet) : "")}</td>
                        <td class='right'>{(v.Kredit > 0 ? fmtNum(v.Kredit) : "")}</td>
                        <td>{(string.IsNullOrWhiteSpace(v.KeteranganVoucher) ? "-" : v.KeteranganVoucher)}</td>
                    </tr>
                ");

                // Mapping ke DTO (Jika dibutuhkan controller untuk Excel dll)
                rawListDto.Add(new VoucherBankDto {
                    NoVoucher = v.NoVoucher,
                    TanggalVoucher = v.TanggalVoucher,
                    KeteranganVoucher = v.KeteranganVoucher,
                    TotalVoucher = v.Debet > 0 ? v.Debet : v.Kredit,
                    DebetKredit = v.Debet > 0 ? "D" : "K"
                });
            }

            // Row Total
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='3' class='right'>TOTAL</td>
                    <td class='right'>{fmtNum(totalDebet)}</td>
                    <td class='right'>{fmtNum(totalKredit)}</td>
                    <td></td>
                </tr>
            ");

            // Row Saldo Akhir
            sb.Append($@"
                <tr class='bold'>
                    <td colspan='6'>Saldo Akhir : {fmtNum(saldoAkhir)}</td>
                </tr>
            ");

            // =========================
            // RENDER SCRIBAN
            // =========================
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "VoucherBank.html");
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Template laporan tidak ditemukan.");

            string htmlTemplate = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(htmlTemplate);

            string rendered = template.Render(new
            {
                details = sb.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                kode_bank = request.KodeBank ?? "-",
                nama_bank = namaBank,
                keterangan_bank = request.KeteranganBank ?? "-"
            });

           return new LaporanVoucherBankResponse
            {
                HtmlString = rendered,
                RawData = rawListDto,
                SaldoAwal = saldoAwal,
                TotalDebet = totalDebet,
                TotalKredit = totalKredit,
                SaldoAkhir = saldoAkhir,
                NamaBank = namaBank
            };
        }
    }
}