using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;

namespace ABB.Application.VoucherKass.Queries
{
    public class GetVoucherKasByIdQuery : IRequest<VoucherKasDto>
    {
        public string NoVoucher { get; set; }
    }

    public class GetVoucherKasByIdQueryHandler : IRequestHandler<GetVoucherKasByIdQuery, VoucherKasDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetVoucherKasByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VoucherKasDto> Handle(GetVoucherKasByIdQuery request, CancellationToken cancellationToken)
        {
           var result = await (
                from vb in _context.VoucherKas
                join kb in _context.KasBank 
                    on vb.KodeAkun equals kb.NoPerkiraan
                join mu in _context.MataUang
                    on vb.KodeMataUang equals mu.kd_mtu
                where vb.NoVoucher == request.NoVoucher
                select new VoucherKasDto
                {
                    NoVoucher = vb.NoVoucher,
                    KodeCabang = vb.KodeCabang,
                    JenisVoucher = vb.JenisVoucher,
                    DebetKredit = vb.DebetKredit,
                    KodeAkun = vb.KodeAkun,
                    DibayarKepada = vb.DibayarKepada,
                    TanggalVoucher = vb.TanggalVoucher,
                    KodeMataUang = vb.KodeMataUang,
                    TotalVoucher = vb.TotalVoucher,
                    TotalDalamRupiah = vb.TotalDalamRupiah,
                    KeteranganVoucher = vb.KeteranganVoucher,
                    FlagPosting = (bool)vb.FlagPosting,
                    JenisPembayaran = vb.JenisPembayaran,
                    TanggalInput = vb.TanggalInput,
                    KodeUserInput = vb.KodeUserInput,
                    TanggalUpdate = vb.TanggalUpdate,
                    KodeUserUpdate = vb.KodeUserUpdate,

                    // dari KasBank
                    NamaKas = kb.Keterangan,

                    // dari MataUang
                    NamaMataUang = mu.symbol,
                    DetailMataUang = mu.nm_mtu
                }
            ).FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}