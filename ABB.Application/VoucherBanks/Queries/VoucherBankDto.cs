using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;

namespace ABB.Application.VoucherBanks.Queries
{
    public class VoucherBankDto : IMapFrom<VoucherBankEntity>
    {
        public long Id { get; set; } // <--- TAMBAHAN
        public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DiterimaDari { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool FlagPosting { get; set; }
        public bool FlagFinal { get; set; }
        public string KodeBank { get; set; }
        public string NoBank { get; set; }
         public string JenisPembayaran { get; set; }

         // --- TAMBAHKAN PROPERTI AUDIT DI SINI ---
        public DateTime? TanggalInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserUpdate { get; set; }

        // definisi
        public string NamaBank { get; set; }
        public string NamaMataUang { get; set; }
        public string DetailMataUang { get; set; }

        public string NamaCabang { get; set; }

        public decimal? Saldo { get; set; }
        public string KeteranganKasBank { get; set; }
        public bool FlagSementara { get; set; }
        public string NoVoucherSementara { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VoucherBankEntity, VoucherBankDto>();
        }
    }
}