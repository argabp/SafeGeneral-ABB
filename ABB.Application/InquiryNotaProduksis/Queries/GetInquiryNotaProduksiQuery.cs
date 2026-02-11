using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class InquiryNotaProduksiQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
         public string JenisAsset { get; set; }

         public string KodeCabang { get; set; }
    }

    public class InquiryNotaProduksiQueryHandler : IRequestHandler<InquiryNotaProduksiQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public InquiryNotaProduksiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(InquiryNotaProduksiQuery request, CancellationToken cancellationToken)
            {
                // var query = _context.Produksi.AsQueryable();
               var query = from p in _context.Produksi
               let normalizedCurensi = (p.curensi.Trim() == "US$" ? "USD" : p.curensi.Trim())
                join m in _context.MataUang on normalizedCurensi equals m.symbol.Trim()
                into mataUangJoin
                from m in mataUangJoin.DefaultIfEmpty() // Gunakan LEFT JOIN

                join k in _context.KeteranganProduksi
                    on p.id equals k.Id
                    into keteranganJoin
                from k in keteranganJoin.DefaultIfEmpty()   // LEFT JOIN

                select new
                {
                    Produksi = p,
                    MataUang = m,
                    Keterangan = k
                };


                // -----------------------------------------------------------
                // 🔹 FILTER KODE CABANG (WAJIB)
                // -----------------------------------------------------------
                if (!string.IsNullOrEmpty(request.KodeCabang))
                {
                    // Filter kolom 'lok' sesuai UserCabang
                    query = query.Where(x => x.Produksi.lok == request.KodeCabang);
                }
                // -----------------------------------------------------------
                
                // 🔹 Filter keyword jika ada
                if (!string.IsNullOrEmpty(request.SearchKeyword))
                {
                    var keyword = request.SearchKeyword.ToLower();
                    query = query.Where(x =>
                        x.Produksi.no_nd.ToLower().Contains(keyword) || // <-- BENAR: 'x.Produksi' adalah AMPLOP
                        x.Produksi.no_ref.ToLower().Contains(keyword) ||
                        x.Produksi.nm_cust2.ToLower().Contains(keyword) || 
                        x.Produksi.no_pl.ToLower().Contains(keyword) 
                    );
                }

                // 🔹 Filter tanggal (opsional)
               if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                    query = query.Where(x => x.Produksi.date >= request.StartDate && x.Produksi.date <= request.EndDate);
                }
                else if (request.StartDate.HasValue)
                {
                    query = query.Where(x => x.Produksi.date >= request.StartDate);
                }
                else if (request.EndDate.HasValue)
                {
                    query = query.Where(x => x.Produksi.date <= request.EndDate);
                }

                if (!string.IsNullOrEmpty(request.JenisAsset))
                {
                    query = query.Where(x => x.Produksi.jn_ass == request.JenisAsset);
                }

               return await query
                .Select(x => new InquiryNotaProduksiDto
                {
                    // Ambil semua properti dari Produksi (x.Produksi)
                    id = x.Produksi.id,
                    no_nd = x.Produksi.no_nd,
                    nm_cust2 = x.Produksi.nm_cust2,
                    nm_cust = x.Produksi.nm_cust,
                    premi = x.Produksi.premi,
                    no_ref = x.Produksi.no_ref,
                    date = x.Produksi.date,
                    type = x.Produksi.type,
                    d_k = x.Produksi.d_k,
                    lok = x.Produksi.lok,
                    no_cust2 = x.Produksi.no_cust2,
                    no_brok = x.Produksi.no_brok,
                    nm_brok = x.Produksi.nm_brok,
                    no_endos = x.Produksi.no_endos,
                    no_pl = x.Produksi.no_pl,
                    curensi = x.Produksi.curensi,
                    kurs = x.Produksi.kurs,
                    no_kwi = x.Produksi.no_kwi,
                    hp = x.Produksi.hp,
                    d_per1 = x.Produksi.d_per1,
                    d_per2 = x.Produksi.d_per2,
                    no_pos = x.Produksi.no_pos,
                    nm_pos = x.Produksi.nm_pos,
                    rabat = x.Produksi.rabat,
                    n_rabat = x.Produksi.n_rabat,
                    n_bruto = x.Produksi.n_bruto,
                    polis = x.Produksi.polis,
                    materai = x.Produksi.materai,
                    komisi = x.Produksi.komisi,
                    n_komisi = x.Produksi.n_komisi,
                    h_fee = x.Produksi.h_fee,
                    n_hfee = x.Produksi.n_hfee,
                    lain = x.Produksi.lain,
                    klaim = x.Produksi.klaim,
                    netto = x.Produksi.netto,
                    tgl_byr = x.Produksi.tgl_byr,
                    jumlah = x.Produksi.jumlah,
                    date_input = x.Produksi.date_input,
                    date_edit = x.Produksi.date_edit,
                    created_by = x.Produksi.created_by,
                    edited_by = x.Produksi.edited_by,
                    jn_ass = x.Produksi.jn_ass,
                    qq = x.Produksi.qq,
                    tgl_jth_tempo = x.Produksi.tgl_jth_tempo,
                    reg = x.Produksi.reg,
                    catat = x.Produksi.catat,
                    kd_ass2 = x.Produksi.kd_ass2,
                    kd_tutup = x.Produksi.kd_tutup,
                    kd_ass3 = x.Produksi.kd_ass3,
                    debet = x.Produksi.debet,
                    nm_pol = x.Produksi.nm_pol,
                    saldo = x.Produksi.saldo,
                    kredit = x.Produksi.kredit,
                    no_bukti = x.Produksi.no_bukti,
                    post_tr = x.Produksi.post_tr,
                    jn_ass2 = x.Produksi.jn_ass2,
                    no_nd2 = x.Produksi.no_nd2,

                    // 🎯 INI YANG PALING PENTING: Ambil dari MataUang (x.MataUang)
                    kd_mtu = (x.MataUang == null ? null : x.MataUang.kd_mtu),
                    keterangan = x.Keterangan != null ? x.Keterangan.Keterangan : null,
                    tanggal_keterangan = x.Keterangan != null ? x.Keterangan.Tanggal : (DateTime?)null
                })
                .ToListAsync(cancellationToken);
            }
    }
}
