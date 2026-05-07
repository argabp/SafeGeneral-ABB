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
                    d_k = x.d_k,
                    netto = x.netto,
                    saldo = x.saldo,
                    no_pl = x.no_pl,           
                    kd_ass2 = x.kd_ass2,       
                    jn_ass = x.jn_ass,         
                    nm_pos = x.nm_pos,         
                    nm_cust2 = x.nm_cust2,     
                    nm_brok = x.nm_brok,       
                    jumlah = x.netto * (x.kurs ?? 1),        
                    date_input = x.date_input  
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (header == null)
                return null;

            var pembayaranBank = await (
                from pb in _context.EntriPembayaranBank
                join vb in _context.VoucherBank
                    on pb.NoVoucher equals vb.NoVoucher
                where pb.NoNota4 == request.NoNota
                select new EntriPembayaranBankDto
                {
                    NoVoucher = vb.NoVoucher,          
                    TanggalVoucher = vb.TanggalVoucher,       
                    TotalDlmRupiah = pb.TotalDlmRupiah,
                    DebetKredit = pb.DebetKredit // <--- PAKAI HURUF BESAR SESUAI ENTITY
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
                    TotalDlmRupiah = pk.TotalDlmRupiah,
                    DebetKredit = pk.DebetKredit // <--- PAKAI HURUF BESAR SESUAI ENTITY
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
                    TotalBayarRp = pp.TotalBayarRp,
                    DebetKredit = pp.DebetKredit // <--- PAKAI HURUF BESAR SESUAI ENTITY
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