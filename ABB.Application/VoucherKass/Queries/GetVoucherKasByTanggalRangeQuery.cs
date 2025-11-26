using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scriban;
using System.IO;

namespace ABB.Application.VoucherKass.Queries
{

       // DTO untuk hasil join VoucherKas + KasBank
    public class VoucherKasJoinDto
    {
        public string NoVoucher { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KeteranganVoucher { get; set; }
        public decimal? TotalVoucher { get; set; }
        public string DibayarKepada { get; set; }
        public string DebetKredit { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string Keterangan { get; set; }
        public decimal? Saldo { get; set; }
    }

    // Query untuk mengambil Voucher KAS dan merender HTML
    public class GetVoucherKasByTanggalRangeQuery : IRequest<string>
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string DatabaseName { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetVoucherKasByTanggalRangeQueryHandler
        : IRequestHandler<GetVoucherKasByTanggalRangeQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetVoucherKasByTanggalRangeQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(GetVoucherKasByTanggalRangeQuery request, CancellationToken cancellationToken)
        {
            // Ambil data voucher KAS + join KasBank
            var vouchers = await (
                from v in _context.VoucherKas
                join k in _context.KasBank
                    on v.KodeAkun equals k.NoPerkiraan
                where v.TanggalVoucher.HasValue
                      && v.TanggalVoucher.Value >= request.TanggalAwal
                      && v.TanggalVoucher.Value <= request.TanggalAkhir
                      && k.Kode == "K01"
                orderby v.TanggalVoucher
                select new VoucherKasJoinDto
                {
                    NoVoucher = v.NoVoucher,
                    TanggalVoucher = v.TanggalVoucher,
                    KeteranganVoucher = v.KeteranganVoucher,
                    TotalVoucher = v.TotalVoucher,
                    DibayarKepada = v.DibayarKepada,
                    DebetKredit = v.DebetKredit,
                    TotalDalamRupiah = v.TotalDalamRupiah,
                    Keterangan = k.Keterangan,
                    Saldo = k.Saldo
                }
            ).ToListAsync(cancellationToken);

            if (!vouchers.Any())
                throw new NullReferenceException("Data Voucher KAS tidak ditemukan.");

            // Helper format
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // Bangun detail baris tabel
            StringBuilder detailsBuilder = new StringBuilder();
            int idx = 1;
            foreach (var v in vouchers)
            {
                detailsBuilder.Append($@"
                    <tr>
                        <td class='center'>{idx}</td>
                        <td>{fmtDate(v.TanggalVoucher)}</td>
                        <td>{v.NoVoucher}</td>
                        <td>{v.DibayarKepada}</td>
                        <td>{v.KeteranganVoucher}</td>
                        <td class='right'>{fmtNum(v.TotalVoucher)}</td>
                        <td class='right'>{fmtNum(v.TotalDalamRupiah)}</td>
                        <td class='right'>{fmtNum(v.Saldo)}</td>
                        <td>{v.Keterangan}</td>
                    </tr>");
                idx++;
            }

            // Path template Scriban
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "VoucherKas.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template VoucherKas.html tidak ditemukan di {templatePath}");

            string templateHtml = await File.ReadAllTextAsync(templatePath, cancellationToken);

            // Render template
            Template template = Template.Parse(templateHtml);

            string resultHtml = template.Render(new
            {
                details = detailsBuilder.ToString(),
                tanggal_awal = request.TanggalAwal.ToString("dd-MM-yyyy"),
                tanggal_akhir = request.TanggalAkhir.ToString("dd-MM-yyyy"),
                user = request.UserLogin
            });

            return resultHtml;
        }
    }
}