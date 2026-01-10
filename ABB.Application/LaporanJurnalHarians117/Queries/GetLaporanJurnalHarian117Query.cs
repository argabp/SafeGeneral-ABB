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
using AutoMapper.QueryableExtensions;
using Scriban;
using Scriban.Runtime;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Dtos;
using ABB.Domain.Entities;
using ABB.Application.Jurnals62.Queries;
using ABB.Application.Coas117.Queries;

namespace ABB.Application.LaporanJurnalHarian117s117.Queries
{
    public class GetLaporanJurnalHarian117Query : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string JenisTransaksi { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetLaporanJurnalHarian117QueryHandler 
        : IRequestHandler<GetLaporanJurnalHarian117Query, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _environment;

        public GetLaporanJurnalHarian117QueryHandler(
            IDbContextPstNota context,
            IMapper mapper,
            IHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<string> Handle(
            GetLaporanJurnalHarian117Query request,
            CancellationToken cancellationToken)
        {
            // ===============================
            // 1. PERIODE (HANYA TANGGAL)
            DateTime tglProdAwal = DateTime.Parse(request.PeriodeAwal).Date; 
            
            // Tambahkan waktu sampai ujung hari untuk tanggal akhir
            DateTime tglProdAkhir = DateTime.Parse(request.PeriodeAkhir).Date
                                            .AddHours(23).AddMinutes(59).AddSeconds(59);

            var db = _context.Set<Jurnal62>()
                             .AsNoTracking()
                             .AsQueryable();

            db = db.Where(j =>
                _context.Set<Coa117>().Any(c =>
                    c.gl_kode == j.GlAkun
                )
            );

            // ===============================
            // 2. FILTER CABANG
            // ===============================
         
            // FILTER KODE CABANG
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var kodeCabangTrimmed = request.KodeCabang.Trim();
                var cabang2Digit = kodeCabangTrimmed.Length >= 2
                    ? kodeCabangTrimmed.Substring(kodeCabangTrimmed.Length - 2, 2) // ambil "50"
                    : kodeCabangTrimmed;

                db = db.Where(x =>
                    !string.IsNullOrEmpty(x.GlLok) &&
                    x.GlLok.Trim() == cabang2Digit);
            }

            // FILTER GLTRAN
            if (!string.IsNullOrEmpty(request.JenisTransaksi))
            {
                var jenisTransaksiTrimmed = request.JenisTransaksi.Trim(); // "MM"

                db = db.Where(x =>
                    !string.IsNullOrEmpty(x.GlTran) &&
                    x.GlTran.Trim() == jenisTransaksiTrimmed
                );
            }

            // ===============================
            // 3. FILTER TANGGAL
            // ===============================
            db = db.Where(x =>
                x.GlTanggal.HasValue &&
                x.GlTanggal.Value >= tglProdAwal &&
                x.GlTanggal.Value <= tglProdAkhir
            );

          
            // ===============================
            // 4. AMBIL NAMA CABANG
            // ===============================
            string namaCabang = "-";
            var cabang = await _context.Set<Cabang>()
                .FirstOrDefaultAsync(x => x.kd_cb == request.KodeCabang, cancellationToken);

            if (cabang != null)
                namaCabang = cabang.nm_cb;

            // ===============================
            // 5. AMBIL DATA JURNAL
            // ===============================
            var dataLaporan = await db
                .ProjectTo<Jurnals62Dto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.GlBukti)
                .ThenBy(x => x.GlTran)
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any())
                throw new Exception("Data jurnal tidak ditemukan.");

            // ===============================
            // 6. BUILD HTML (GROUP BY GL_BUKTI)
            // ===============================
            StringBuilder detailsBuilder = new StringBuilder();

            foreach (var group in dataLaporan
                .GroupBy(x => x.GlBukti ?? "TANPA JURNAL")
                .OrderBy(g => g.Key))
            {
                // ---- HEADER NO JURNAL ----
                detailsBuilder.Append($@"
                <tr>
                    <td colspan='8' style='font-weight:bold; padding-top:10px;'>
                        No. Jurnal : {group.Key}
                    </td>
                </tr>
                ");

                decimal subDebet = 0;
                decimal subKredit = 0;
                 var idx = 1;

                foreach (var item in group)
                {
                    decimal nilaiIdr = item.GlNilaiIdr ?? 0;
                    decimal nilaiOrg = item.GlNilaiOrg ?? 0;

                    decimal debetIdr  = item.GlDk == "D" ? nilaiIdr : 0;
                    decimal kreditIdr = item.GlDk == "K" ? nilaiIdr : 0;

                    decimal debetOrg  = item.GlDk == "D" ? nilaiOrg : 0;
                    decimal kreditOrg = item.GlDk == "K" ? nilaiOrg : 0;

                    detailsBuilder.Append($@"
                    <tr>
                        <td>{idx}</td>
                        <td>{item.GlTanggal:dd/MM}</td>
                        <td>{item.GlAkun}</td>
                        <td>{item.GlMtu}</td>
                        <td style='text-align:right'>{nilaiOrg}</td>
                        <td style='text-align:right'>{debetIdr:N2}</td>
                        <td style='text-align:right'>{kreditIdr:N2}</td>
                        <td>{item.GlKet}</td>
                    </tr>
                    ");

                    subDebet  += debetIdr;
                    subKredit += kreditIdr;
                    idx++;
                }

                // ---- SUB TOTAL ----
                detailsBuilder.Append($@"
                <tr style='font-weight:bold;'>
                    <td colspan='5' style='text-align:right'>Sub Total :</td>
                    <td style='text-align:right'>{subDebet:N2}</td>
                    <td style='text-align:right'>{subKredit:N2}</td>
                    <td></td>
                </tr>
                ");
            }

            // ===============================
            // 7. RENDER TEMPLATE SCRIBAN
            // ===============================
            string templatePath = Path.Combine(
                _environment.ContentRootPath,
                "Modules", "Reports", "Templates", "LaporanJurnalHarian117.html");

            string templateHtml = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateHtml);

            var model = new
            {
                details = detailsBuilder.ToString(),
                KodeCabang = request.KodeCabang,
                NamaCabang = namaCabang,
                PeriodeAwal = tglProdAwal.ToString("dd-MM-yyyy"),
                PeriodeAkhir = tglProdAkhir.ToString("dd-MM-yyyy"),
                TanggalCetak = DateTime.Now.ToString("dd-MM-yyyy"),
                JudulLaporan = "LAPORAN JURNAL HARIAN"
            };

            var ctx = new TemplateContext();
            var script = new ScriptObject();
            script.Import(model, renamer: m => m.Name);
            ctx.PushGlobal(script);

            return template.Render(ctx);
        }
    }
}
