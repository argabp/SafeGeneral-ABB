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

            // 2. Filter Cabang (2 Digit)
            string cabang2Digit = "";
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var trimCabang = request.KodeCabang.Trim();
                cabang2Digit = trimCabang.Length >= 2 ? trimCabang.Substring(trimCabang.Length - 2, 2) : trimCabang;
            }

            // 3. Query Dasar Jurnal
            var dbJurnal = _context.Set<Jurnal62>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(cabang2Digit))
            {
                dbJurnal = dbJurnal.Where(x => x.GlLok == cabang2Digit);
            }

            // 4. Ambil Akun yang terlibat
            var queryAkun = _context.Set<Coa>().AsNoTracking().AsQueryable();
            
            // [PERBAIKAN]: Menggunakan gl_kode sesuai mapping DTO
            if (!string.IsNullOrEmpty(request.AkunAwal))
                queryAkun = queryAkun.Where(x => x.gl_kode.CompareTo(request.AkunAwal) >= 0);
            
            if (!string.IsNullOrEmpty(request.AkunAkhir))
                queryAkun = queryAkun.Where(x => x.gl_kode.CompareTo(request.AkunAkhir) <= 0);

            // Kita select ke Anonymous Object dulu
            var listAkun = await queryAkun
                .OrderBy(x => x.gl_kode)
                .Select(x => new 
                { 
                    Kode = x.gl_kode, // Pakai gl_kode
                    Nama = x.gl_nama  // Pakai gl_nama
                })
                .ToListAsync(cancellationToken);

            if (!listAkun.Any()) throw new Exception("Tidak ada akun yang ditemukan dalam range tersebut.");

            // 5. Build HTML
            StringBuilder sb = new StringBuilder();

            foreach (var akun in listAkun)
            {
                string kodeAkun = (akun.Kode ?? "").Trim();
                string namaAkun = (akun.Nama ?? "").Trim();

                // --- A. Hitung Saldo Awal ---
                var saldoAwalQuery = dbJurnal.Where(x => x.GlAkun == kodeAkun && x.GlTanggal < tglAwal);
                var sumDebetAwal = await saldoAwalQuery.Where(x => x.GlDk == "D").SumAsync(x => x.GlNilaiIdr ?? 0, cancellationToken);
                var sumKreditAwal = await saldoAwalQuery.Where(x => x.GlDk == "K").SumAsync(x => x.GlNilaiIdr ?? 0, cancellationToken);
                
                decimal saldoAwal = sumDebetAwal - sumKreditAwal;

                // --- B. Ambil Transaksi Periode Ini ---
                var transaksiList = await dbJurnal
                    .Where(x => x.GlAkun == kodeAkun && x.GlTanggal >= tglAwal && x.GlTanggal <= tglAkhir)
                    .OrderBy(x => x.GlTanggal).ThenBy(x => x.GlBukti)
                    .Select(x => new 
                    {
                        x.GlTanggal,
                        x.GlBukti,
                        x.GlKet,
                        x.GlDk,
                        Nilai = x.GlNilaiIdr ?? 0
                    })
                    .ToListAsync(cancellationToken);

                if (saldoAwal == 0 && !transaksiList.Any()) continue;

                // --- C. Render Row Transaksi ---
                decimal totalDebetBulanIni = 0;
                decimal totalKreditBulanIni = 0;
                
                // Flag untuk menandai baris pertama
                bool isFirstRow = true; 

                foreach (var item in transaksiList)
                {
                    decimal debet = item.GlDk == "D" ? item.Nilai : 0;
                    decimal kredit = item.GlDk == "K" ? item.Nilai : 0;

                    // LOGIC: Tampilkan Akun & Nama HANYA di baris pertama
                    string displayKode = isFirstRow ? kodeAkun : "";
                    string displayNama = isFirstRow ? namaAkun : "";
                    
                    // LOGIC: Tambahkan class 'border-top-black' HANYA di baris pertama
                    // Ini akan membuat garis pemisah antar akun
                    string rowClass = isFirstRow ? "border-top-black" : "";

                    sb.Append($@"
                    <tr class='{rowClass}'>
                        <td style='font-weight:bold;'>{displayKode}</td>
                        <td style='font-weight:bold;'>{displayNama}</td>
                        
                        <td class='center'>{item.GlTanggal:dd/MM/yyyy}</td>
                        <td>{item.GlBukti}</td>
                        <td>{item.GlKet}</td>
                        <td class='right'>{debet:N2}</td>
                        <td class='right'>{kredit:N2}</td>
                    </tr>");

                    totalDebetBulanIni += debet;
                    totalKreditBulanIni += kredit;
                    
                    // Set false agar baris berikutnya tidak ada garis & nama akun
                    isFirstRow = false; 
                }
                
                // Jika tidak ada transaksi tapi ada saldo awal, kita perlu render 1 baris kosong
                // supaya nama akun tetap muncul (Kasus jarang, tapi perlu dihandle)
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

                // --- D. Hitung Footer Summary ---
                decimal selisihBulanBerjalan = totalDebetBulanIni - totalKreditBulanIni;
                decimal saldoAkhir = saldoAwal + selisihBulanBerjalan;

                // --- LOGIC PENENTUAN KOLOM DEBET/KREDIT ---
                
                // 1. Saldo Awal
                string saDebet = saldoAwal >= 0 ? saldoAwal.ToString("N2") : "0.00";
                string saKredit = saldoAwal < 0 ? Math.Abs(saldoAwal).ToString("N2") : "0.00";

                // 2. Selisih
                string selisihDebet = selisihBulanBerjalan >= 0 ? selisihBulanBerjalan.ToString("N2") : "0.00";
                string selisihKredit = selisihBulanBerjalan < 0 ? Math.Abs(selisihBulanBerjalan).ToString("N2") : "0.00";

                // 3. Saldo Akhir
                string sakDebet = saldoAkhir >= 0 ? saldoAkhir.ToString("N2") : "0.00";
                string sakKredit = saldoAkhir < 0 ? Math.Abs(saldoAkhir).ToString("N2") : "0.00";


                // --- E. Render Footer ---
                string labelStyle = "display:inline-block; width:180px; text-align:left;";

                // Baris 1: Jumlah (Total Mutasi) - Ini selalu positif karena sum murni
                sb.Append($@"
                <tr class='footer-row'>
                    <td colspan='5' class='right'>
                        <div style='{labelStyle}'>*** J u m l a h </div>
                    </td>
                    <td class='right bt-black'>{totalDebetBulanIni:N2}</td>
                    <td class='right bt-black'>{totalKreditBulanIni:N2}</td>
                </tr>");

                // Baris 2: Saldo Awal (Pecah jadi 2 kolom: Debet & Kredit)
                sb.Append($@"
                <tr class='data-2'>
                    <td colspan='5' class='right'>
                        <div style='{labelStyle}'>*** Saldo s/d Bulan Lalu</div>
                    </td>
                    <td class='right'>{saDebet}</td>
                    <td class='right'>{saKredit}</td>
                </tr>");

                // Baris 3: Selisih (Pecah jadi 2 kolom)
                sb.Append($@"
                <tr class='data-3'>
                    <td colspan='5' class='right'>
                         <div style='{labelStyle}'>*** Selisih Bulan Berjalan</div>
                    </td>
                    <td class='right'>{selisihDebet}</td>
                    <td class='right'>{selisihKredit}</td>
                </tr>");

                // Baris 4: Saldo Akhir (Pecah jadi 2 kolom)
                sb.Append($@"
                <tr class='data-4' style='font-weight:bold;'>
                    <td colspan='5' class='right'>
                         <div style='{labelStyle}'>*** Saldo s/d Bulan Ini</div>
                    </td>
                    <td class='right'>{sakDebet}</td>
                    <td class='right'>{sakKredit}</td>
                </tr>
               
                ");
            }

            // 6. Load Template
            string templatePath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanBukuBesar.html");
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Template LaporanBukuBesar.html tidak ditemukan");

            string templateHtml = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateHtml);

            var model = new
            {
                details = sb.ToString(),
                PeriodeAwal = tglAwal.ToString("dd-MM-yyyy"), // Format diperbaiki jadi dd-MM-yyyy biar seragam
                PeriodeAkhir = tglAkhir.ToString("dd-MM-yyyy"),
                NamaCabang = request.KodeCabang
            };

            // --- [PERBAIKAN UTAMA DISINI] ---
            // Kita pakai TemplateContext supaya nama variabel 'PeriodeAwal' tidak berubah jadi 'periode_awal'
            var ctx = new TemplateContext();
            var script = new ScriptObject();
            
            // renamer: m => m.Name artinya: Gunakan nama asli properti C# (Case Sensitive)
            script.Import(model, renamer: m => m.Name); 
            
            ctx.PushGlobal(script);

            return await template.RenderAsync(ctx);
        }
    }
}