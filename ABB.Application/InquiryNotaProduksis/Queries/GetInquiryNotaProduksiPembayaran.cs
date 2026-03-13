using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Application.EntriPembayaranBanks.Queries;
using ABB.Application.EntriPembayaranKass.Queries;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class GetInquiryNotaProduksiPembayaranQuery 
        : IRequest<InquiryNotaProduksiPembayaranDto>
    {
        public string NoNota { get; set; }
    }

    public class GetInquiryNotaProduksiPembayaranQueryHandler
        : IRequestHandler<GetInquiryNotaProduksiPembayaranQuery, InquiryNotaProduksiPembayaranDto>
    {
        private readonly IDbContextPstNota _context;

        public GetInquiryNotaProduksiPembayaranQueryHandler(
            IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<InquiryNotaProduksiPembayaranDto> Handle(
            GetInquiryNotaProduksiPembayaranQuery request,
            CancellationToken cancellationToken)
        {
            // 🔹 HEADER NOTA
            var header = await _context.Produksi
                .Where(x => x.no_nd == request.NoNota)
                .Select(x => new InquiryNotaProduksiDto
                {
                    id = x.id,
                    no_nd = x.no_nd,
                    no_ref = x.no_ref,
                    nm_cust = x.nm_cust,
                    date = x.date,
                    netto = x.netto,
                    saldo = x.saldo,
                    no_pl = x.no_pl,           // NO. POLIS
                    kd_ass2 = x.kd_ass2,       // COB
                    jn_ass = x.jn_ass,         // COB (Cadangan)
                    nm_pos = x.nm_pos,         // PEMBAWA POS
                    nm_cust2 = x.nm_cust2,     // CEDING
                    nm_brok = x.nm_brok,       // AGEN/BROKER
                    jumlah = x.jumlah * (x.kurs ?? 1),        // JUMLAH BIAYA
                    date_input = x.date_input  // TANGGAL NOT
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (header == null)
                return null;

            // 🔹 PEMBAYARAN BANK
            var pembayaranBank = await (
                from pb in _context.EntriPembayaranBank
                join vb in _context.VoucherBank
                    on pb.NoVoucher equals vb.NoVoucher
                where pb.NoNota4 == request.NoNota
                select new EntriPembayaranBankDto
                {
                    NoVoucher = vb.NoVoucher,          // dari VoucherBank
                    TanggalVoucher = vb.TanggalVoucher,       // dari VoucherBank
                    TotalDlmRupiah = pb.TotalDlmRupiah       // dari PembayaranBank
                }
            ).ToListAsync(cancellationToken);

            // 🔹 PEMBAYARAN KAS
           var pembayaranKas = await (
                from pk in _context.EntriPembayaranKas
                join vk in _context.VoucherKas
                    on pk.NoVoucher equals vk.NoVoucher
                where pk.NoNota4 == request.NoNota
                select new EntriPembayaranKasDto
                {
                    NoVoucher = vk.NoVoucher,
                    TanggalVoucher = vk.TanggalVoucher,
                    TotalDlmRupiah = pk.TotalDlmRupiah
                }
            ).ToListAsync(cancellationToken);

            // 🔹 PEMBAYARAN PIUTANG
           var pembayaranPiutang = await (
                from pp in _context.EntriPenyelesaianPiutang
                join vp in _context.HeaderPenyelesaianUtang
                    on pp.NoBukti equals vp.NomorBukti
                where pp.NoNota == request.NoNota
                select new HeaderPenyelesaianUtangDto
                {
                    NomorBukti = vp.NomorBukti,
                    Tanggal = vp.Tanggal,
                    TotalBayarRp = pp.TotalBayarRp
                }
            ).ToListAsync(cancellationToken);

            return new InquiryNotaProduksiPembayaranDto
            {
                Header = header,
                PembayaranBank = pembayaranBank,
                PembayaranKas = pembayaranKas,
                PembayaranPiutang = pembayaranPiutang
            };
        }
    }
}
