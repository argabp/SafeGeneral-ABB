using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using ABB.Application.InquiryNotaProduksis.Queries; // Kita masih butuh DTO-nya

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    // 1. QUERY BARU
    public class GetNotaUntukPembayaranQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string JenisAsset { get; set; }
    }

    // 2. HANDLER BARU
    public class GetNotaUntukPembayaranQueryHandler : IRequestHandler<GetNotaUntukPembayaranQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetNotaUntukPembayaranQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(GetNotaUntukPembayaranQuery request, CancellationToken cancellationToken)
        {
            // JOIN Anda yang sudah benar
            var query = from p in _context.Produksi
                        let normalizedCurensi = (p.curensi.Trim() == "US$" ? "USD" : p.curensi.Trim())
                        join m in _context.MataUang on normalizedCurensi equals m.symbol.Trim()
                        into mataUangJoin
                        from m in mataUangJoin.DefaultIfEmpty()
                        select new { Produksi = p, MataUang = m };

            // --- 🔽 FILTER WAJIB BARU 🔽 ---
            // Ini adalah filter saldo yang Anda inginkan
            query = query.Where(x => x.Produksi.saldo.HasValue && x.Produksi.saldo > 0);
            // ---------------------------------

            // Filter keyword
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(x =>
                    x.Produksi.no_nd.ToLower().Contains(keyword) ||
                    x.Produksi.no_ref.ToLower().Contains(keyword) ||
                    x.Produksi.nm_cust2.ToLower().Contains(keyword) ||
                    x.Produksi.no_pl.ToLower().Contains(keyword)
                );
            }

            // Filter tanggal
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
            
            // Filter jenis asset
            if (!string.IsNullOrEmpty(request.JenisAsset))
            {
                query = query.Where(x => x.Produksi.jn_ass == request.JenisAsset);
            }

            // Select manual Anda yang sudah benar
            return await query
                .Select(x => new InquiryNotaProduksiDto
                {
                    // ... (semua properti Anda)
                    id = x.Produksi.id,
                    no_nd = x.Produksi.no_nd,
                    nm_cust2 = x.Produksi.nm_cust2,
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
                    
                    kd_mtu = (x.MataUang == null ? null : x.MataUang.kd_mtu)
                })
                .ToListAsync(cancellationToken);
        }
    }
}