using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Scriban;
using Scriban.Runtime;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;

namespace ABB.Application.LaporanBukuBesars.Queries
{
    public class GetLaporanBukuBesarQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string AkunAwal { get; set; }
        public string AkunAkhir { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetLaporanBukuBesarQueryHandler : IRequestHandler<GetLaporanBukuBesarQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanBukuBesarQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanBukuBesarQuery request, CancellationToken cancellationToken)
        {
            // 1. Parsing Periode
            DateTime tglAwal = DateTime.Parse(request.PeriodeAwal).Date;
            DateTime tglAkhir = DateTime.Parse(request.PeriodeAkhir).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // 2. Eksekusi Stored Procedure
            // Menggunakan BukuBesarSpDto yang sudah didaftarkan di DbContext
            // Pastikan urutan parameter sesuai dengan di SQL Server
           var rawData = await _context.BukuBesarSpResults 
                .FromSqlRaw("EXEC sp_LaporanBukuBesar {0}, {1}, {2}, {3}, {4}",
                    request.KodeCabang ?? "",
                    tglAwal,
                    tglAkhir,
                    request.AkunAwal ?? "",
                    request.AkunAkhir ?? "ZZZZZ"
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!rawData.Any()) throw new Exception("Data tidak ditemukan dalam periode tersebut.");

            // 3. Grouping Data (Karena SP mengembalikan flat list)
            // Kita group berdasarkan Kode Akun agar bisa di-loop seperti sebelumnya
            var groupedData = rawData.GroupBy(x => x.KodeAkun);

            // 4. Build HTML
            StringBuilder sb = new StringBuilder();

            foreach (var group in groupedData)
            {
                // Ambil informasi header dari baris pertama di grup
                var headerInfo = group.First();
                string kodeAkun = headerInfo.KodeAkun;
                string namaAkun = headerInfo.NamaAkun;
                decimal saldoAwal = headerInfo.SaldoAwal ?? 0;

                decimal totalDebetBulanIni = 0;
                decimal totalKreditBulanIni = 0;
                
                // Flag untuk menandai baris pertama (untuk border & nama akun)
                bool isFirstRow = true;

                // Ambil Transaksi (Filter RowType = 1, asumsikan SP mengembalikan 1 untuk transaksi)
                // Jika logic SP kamu mengembalikan semua baris termasuk saldo awal sebagai baris sendiri, sesuaikan filter ini.
                // Disini asumsinya SP mengembalikan detail transaksi dengan RowType=1
                var transaksiList = group.Where(x => x.RowType == 1).OrderBy(x => x.Tanggal).ThenBy(x => x.NoBukti).ToList();

                foreach (var item in transaksiList)
                {
                    decimal debet = item.Debet ?? 0;
                    decimal kredit = item.Kredit ?? 0;

                    // LOGIC: Tampilkan Akun & Nama HANYA di baris pertama
                    string displayKode = isFirstRow ? kodeAkun : "";
                    string displayNama = isFirstRow ? namaAkun : "";
                    
                    // LOGIC: Border Top Hitam HANYA di baris pertama
                    string rowClass = isFirstRow ? "border-top-black" : "";

                    sb.Append($@"
                    <tr class='{rowClass}'>
                        <td style='font-weight:bold;'>{displayKode}</td>
                        <td style='font-weight:bold;'>{displayNama}</td>
                        
                        <td class='center'>{(item.Tanggal.HasValue ? item.Tanggal.Value.ToString("dd/MM/yyyy") : "")}</td>
                        <td>{item.NoBukti}</td>
                        <td>{item.Keterangan}</td>
                        <td class='right'>{debet:N2}</td>
                        <td class='right'>{kredit:N2}</td>
                    </tr>");

                    totalDebetBulanIni += debet;
                    totalKreditBulanIni += kredit;
                    
                    isFirstRow = false; 
                }
                
                // Jika tidak ada transaksi tapi ada saldo awal (RowType 1 kosong, tapi RowType 0/Header ada saldo)
                if (transaksiList.Count == 0 && saldoAwal != 0)
                {
                     sb.Append($@"
                    <tr>
                        <td style='font-weight:bold;'>{kodeAkun}</td>
                        <td style='font-weight:bold;'>{namaAkun}</td>
                        <td class='center'>-</td>
                        <td>-</td>
                        <td>Saldo Awal</td>
                        <td class='right'>0.00</td>
                        <td class='right'>0.00</td>
                    </tr>");
                }

                // --- Hitung Footer Summary ---
                decimal selisihBulanBerjalan = totalDebetBulanIni - totalKreditBulanIni;
                decimal saldoAkhir = saldoAwal + selisihBulanBerjalan;

                // --- LOGIC PENENTUAN KOLOM DEBET/KREDIT FOOTER ---
                string saDebet = saldoAwal >= 0 ? saldoAwal.ToString("N2") : "0.00";
                string saKredit = saldoAwal < 0 ? Math.Abs(saldoAwal).ToString("N2") : "0.00";

                string selisihDebet = selisihBulanBerjalan >= 0 ? selisihBulanBerjalan.ToString("N2") : "0.00";
                string selisihKredit = selisihBulanBerjalan < 0 ? Math.Abs(selisihBulanBerjalan).ToString("N2") : "0.00";

                string sakDebet = saldoAkhir >= 0 ? saldoAkhir.ToString("N2") : "0.00";
                string sakKredit = saldoAkhir < 0 ? Math.Abs(saldoAkhir).ToString("N2") : "0.00";

                string labelStyle = "display:inline-block; width:180px; text-align:left;";

                // Render Footer
                sb.Append($@"
                <tr class='footer-row'>
                    <td colspan='5' class='right'>
                        <div style='{labelStyle}'>*** J u m l a h </div>
                    </td>
                    <td class='right bt-black'>{totalDebetBulanIni:N2}</td>
                    <td class='right bt-black'>{totalKreditBulanIni:N2}</td>
                </tr>
                <tr class='data-2'>
                    <td colspan='5' class='right'>
                        <div style='{labelStyle}'>*** Saldo s/d Bulan Lalu</div>
                    </td>
                    <td class='right'>{saDebet}</td>
                    <td class='right'>{saKredit}</td>
                </tr>
                <tr class='data-3'>
                    <td colspan='5' class='right'>
                         <div style='{labelStyle}'>*** Selisih Bulan Berjalan</div>
                    </td>
                    <td class='right'>{selisihDebet}</td>
                    <td class='right'>{selisihKredit}</td>
                </tr>
                <tr class='data-4' style='font-weight:bold;'>
                    <td colspan='5' class='right'>
                         <div style='{labelStyle}'>*** Saldo s/d Bulan Ini</div>
                    </td>
                    <td class='right'>{sakDebet}</td>
                    <td class='right'>{sakKredit}</td>
                </tr>
                <tr><td colspan='7' style='border:none; height:15px;'></td></tr>
                ");
            }

            // 5. Load Template & Render (Bagian ini TETAP SAMA)
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanBukuBesar.html");
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Template LaporanBukuBesar.html tidak ditemukan");

            string templateHtml = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateHtml);

            var model = new
            {
                details = sb.ToString(),
                PeriodeAwal = tglAwal.ToString("dd-MM-yyyy"),
                PeriodeAkhir = tglAkhir.ToString("dd-MM-yyyy"),
                NamaCabang = request.KodeCabang
            };

            var ctx = new TemplateContext();
            var script = new ScriptObject();
            script.Import(model, renamer: m => m.Name); 
            ctx.PushGlobal(script);

            return await template.RenderAsync(ctx);
        }
    }
}