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
        
        // Data dari KasBank
        public string KodeKas { get; set; }     // Tambahan: Biar tau ini K01 atau K02
        public string NamaKas { get; set; }     // Keterangan dari tabel KasBank
        public decimal? Saldo { get; set; }
        public string KodeCabang { get; set; } 
    }

    public class GetVoucherKasByTanggalRangeQuery : IRequest<string>
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public string DatabaseName { get; set; }
        public string UserLogin { get; set; }
        public string KodeKas { get; set; } // filter opsional
        public string KeteranganKas { get; set; }
        public string KodeCabang { get; set; }
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
            // PERBAIKAN: Join berdasarkan KodeKas yang tersimpan di VoucherKas (v.KodeKas)
            // agar lebih akurat daripada join by Akun (karena 1 akun bisa dipake >1 kode kas walau jarang)
            
            var vouchers = await (
                from v in _context.VoucherKas
                join k in _context.KasBank
                    on v.KodeKas equals k.Kode // Join langsung by Kode Kas (K01, K02)
                where v.TanggalVoucher.HasValue
                      && v.TanggalVoucher.Value >= request.TanggalAwal
                      && v.TanggalVoucher.Value <= request.TanggalAkhir
                      // && k.Kode == "K01" <--- INI DIHAPUS SUPAYA SEMUA KAS MUNCUL
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
                    KodeCabang = v.KodeCabang,
                    KodeKas = v.KodeKas,       // Tampilkan Kodenya
                    NamaKas = k.Keterangan,    // Tampilkan Namanya
                    Saldo = k.Saldo
                }
            ).ToListAsync(cancellationToken);

            if (!vouchers.Any())
                // Jangan throw error, return HTML kosong/pesan saja biar user gak kaget error page
                // Tapi kalau requirementmu throw, biarkan saja.
                throw new NullReferenceException("Data Voucher KAS tidak ditemukan pada periode tersebut.");

            if (!string.IsNullOrEmpty(request.KodeKas))
                {
                    vouchers = vouchers
                        .Where(v => v.KodeKas == request.KodeKas)
                        .ToList();
                }

                
            if (!string.IsNullOrEmpty(request.KodeCabang))
                {
                    vouchers = vouchers
                        .Where(v => v.KodeCabang == request.KodeCabang)
                        .ToList();
                }

            // Helper format
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";

            // Bangun detail baris tabel
            StringBuilder detailsBuilder = new StringBuilder();
            int idx = 1;
            foreach (var v in vouchers)
            {
                // Disini saya tambahkan kolom Kode Kas di tampilan (opsional, sesuaikan HTML header kamu)
                detailsBuilder.Append($@"
                    <tr>
                        <td class='center'>{idx}</td>
                        <td class='center'>{v.KodeKas}</td> <td>{fmtDate(v.TanggalVoucher)}</td>
                        <td>{v.NoVoucher}</td>
                        <td>{v.DibayarKepada}</td>
                        <td>{v.KeteranganVoucher}</td>
                        <td class='right'>{fmtNum(v.TotalVoucher)}</td>
                        <td class='right'>{fmtNum(v.TotalDalamRupiah)}</td>
                        <td class='right'>{fmtNum(v.Saldo)}</td>
                    </tr>");
                idx++;
            }

            // Path template Scriban
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "VoucherKas.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template VoucherKas.html tidak ditemukan di {templatePath}");

            string templateHtml = await File.ReadAllTextAsync(templatePath, cancellationToken);

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